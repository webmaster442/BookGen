using System.Text.Json;
using System.Text.Json.Nodes;

//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Vfs;

namespace Bookgen.Lib.Confighandling;

internal sealed class JsonMerger
{
    private JsonObject _baseObject;

    public JsonMerger(JsonObject baseObject)
    {
        _baseObject = baseObject;
    }

    private static JsonNode? Merge(JsonNode jsonBase,
                                   JsonNode jsonMerge,
                                   bool mergeIfAlreadyExists = true)
    {
        if (jsonBase == null || jsonMerge == null)
            return jsonBase;

        switch (jsonBase)
        {
            case JsonObject jsonBaseObj when jsonMerge is JsonObject jsonMergeObj:
                {
                    //NOTE: We must materialize the set (e.g. to an Array), and then clear the merge array so the node can then be 
                    //      re-assigned to the target/base Json; clearing the Object seems to be the most efficient approach...
                    var mergeNodesArray = jsonMergeObj.ToArray();
                    jsonMergeObj.Clear();

                    foreach (var prop in mergeNodesArray)
                    {
                        if (mergeIfAlreadyExists || !jsonBaseObj.ContainsKey(prop.Key))
                            jsonBaseObj[prop.Key] = jsonBaseObj[prop.Key] switch
                            {
                                JsonObject jsonBaseChildObj when prop.Value is JsonObject jsonMergeChildObj 
                                    => Merge(jsonBaseChildObj, jsonMergeChildObj),
                                JsonArray jsonBaseChildArray when prop.Value is JsonArray jsonMergeChildArray
                                    => Merge(jsonBaseChildArray, jsonMergeChildArray),
                                _ => prop.Value
                            };
                    }
                    break;
                }
            case JsonArray jsonBaseArray when jsonMerge is JsonArray jsonMergeArray:
                {
                    //NOTE: We must materialize the set (e.g. to an Array), and then clear the merge array,
                    //      so they can then be re-assigned to the target/base Json...
                    var mergeNodesArray = jsonMergeArray.ToArray();
                    jsonMergeArray.Clear();
                    foreach (var mergeNode in mergeNodesArray) jsonBaseArray.Add(mergeNode);
                    break;
                }
            default:
                throw new ArgumentException($"The JsonNode type [{jsonBase.GetType().Name}] is incompatible for merging with the target/base " +
                                            $"type [{jsonMerge.GetType().Name}]; merge requires the types to be the same.");

        }

        return jsonBase;
    }


    public void Merge(JsonObject overlay)
    {
        var result = Merge(_baseObject, overlay);
        if (result is JsonObject mergedObj)
        {
            _baseObject = mergedObj;
            return;
        }
        throw new InvalidOperationException("Merging resulted in invalid object");
    }

    public T? Deserialize<T>() 
        => JsonSerializer.Deserialize<T>(_baseObject, JsonOptions.SerializerOptions);
}
