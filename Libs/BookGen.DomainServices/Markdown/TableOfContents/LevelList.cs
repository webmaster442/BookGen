//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.DomainServices.Markdown.TableOfContents;

/// <summary>
/// Sort and merge with level
/// </summary>
/// <typeparam name="T">LevelList`T</typeparam>
internal class LevelList<T> where T : LevelList<T>, new()
{
    /// <summary>
    /// Current item for append
    /// </summary>
    protected T? _current;
    /// <summary>
    /// The actual data list
    /// </summary>
    protected readonly List<T> _data = new();

    /// <summary>
    /// Current Level
    /// </summary>
    public int Level { get; set; }
    /// <summary>
    /// Is locator, just take place, no renderer
    /// </summary>
    public bool IsLocator { get; set; } = false;
    /// <summary>
    /// Parent of this
    /// </summary>
    public T? Parent { get; protected set; }
    /// <summary>
    /// Current count of children
    /// </summary>
    public int Count => _data.Count;
    /// <summary>
    /// Children of this list
    /// </summary>
    public List<T> Children => _data;
    /// <summary>
    /// Get item by index
    /// </summary>
    /// <param name="index">index of child</param>
    /// <returns></returns>
    public T this[int index]
    {
        get => _data[index];
        set => _data[index] = value;
    }

    /// <summary>
    /// Clear this list
    /// </summary>
    public void Clear()
    {
        _current = null;
        for (int i = 0; i < _data.Count; i++)
        {
            T? item = _data[i];
            if (item != null)
                item.Parent = null;
        }

        _data.Clear();
    }

    private static T FindChildLevelLessThan(int level, T current)
    {
        if (current._current == null || current._current.Level >= level)
            return current;
        return LevelList<T>.FindChildLevelLessThan(level, current._current);
    }

    private static T? FindParentLevelLessThan(int level, T current)
    {
        if (current.Parent == null)
            return null;
        if (current.Parent.Level < level)
            return current.Parent;
        return LevelList<T>.FindParentLevelLessThan(level, current.Parent);
    }

    /// <summary>
    /// Add a item to list , append or merge.
    /// </summary>
    /// <param name="item">The item to add</param>
    public void Append(T item)
    {
        //at the beginning
        if (_current is null)
        {
            _data.Add(item);
            item.Parent = this as T;
            _current = item; //move to the last one
            return;
        }

        // continue append
        if (item.Level > _current.Level)
        {
            //try to find last child that child.Level > item.Level
            var found = LevelList<T>.FindChildLevelLessThan(item.Level, _current);
            var offset = item.Level - found.Level;
            if (offset > 1)
            {
                T current = found;
                //create empy Locator
                for (int i = 1; i < offset; i++)
                {
                    var emtpy = new T()
                    {
                        IsLocator = true,
                        Level = found.Level + i,
                        Parent = current
                    };
                    current._data.Add(emtpy);
                    current._current = emtpy;
                    current = emtpy;
                }
                found = current;
            }
            found._data.Add(item);
            found._current = item;
            item.Parent = found;
            return;
        }

        //not find a right item, move to previous one
        var parent = LevelList<T>.FindParentLevelLessThan(item.Level, _current);
        if (parent is not null)
        {
            //marge siblings which
            MargeSiblings(parent, item);
            return;
        }

        Debug.WriteLine("Cannot find a parent.");
        //not yet added, reached root (i.e. Parent is null)
        //set root level and do marge
        Level = item.Level - 1;
        MargeSiblings((T)this, item);
    }

    /// <summary>
    /// Marge sibilings
    /// </summary>
    /// <param name="parent">Which to add in</param>
    /// <param name="add">The item to add</param>
    protected virtual void MargeSiblings(T parent, T add)
    {
        var list = new List<int[]>();
        int startAt = -1;//include
        int endBefore = -1;//not include

        for (int i = 0; i < parent.Count; i++)
        {
            var t = parent[i];
            if (t.Level > add.Level)
            {
                if (startAt == -1)
                    startAt = i;
            }
            else if (startAt != -1)
            {
                endBefore = i;
                list.Add([startAt, endBefore]);
                startAt = -1;
            }
        }

        if (startAt != -1)
        {
            endBefore = parent.Count;
            list.Add([startAt, endBefore]);
        }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            var index = list[i];
            int start = index[0];
            int end = index[1];

            var emtpy = new T()
            {
                IsLocator = true,
                Level = add.Level,
                Parent = parent
            };
            for (int k = start; k < end; k++)
            {
                var item = parent[k];
                item.Parent = emtpy;
                emtpy._data.Add(item);
                if (start == end - 1)
                    emtpy._current = item;
            }
            if (end - start > 1
                && parent._data.Count > 0)
            {
                parent._data.RemoveRange(start + 1, end - start - 1);
            }

            parent._data[start] = emtpy;
        }

        parent._data.Add(add);
        add.Parent = parent;
        parent._current = add;
    }
}
