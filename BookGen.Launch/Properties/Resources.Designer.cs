﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookGen.Launch.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BookGen.Launch.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to clear the recent files list?.
        /// </summary>
        internal static string ClearRecentList {
            get {
                return ResourceManager.GetString("ClearRecentList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Selected folder can&apos;t be located anymore. Probably it was deleted. It will be removed from the recent folders list..
        /// </summary>
        internal static string FolderNoLongerExists {
            get {
                return ResourceManager.GetString("FolderNoLongerExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select a folder to start bookgen shell.
        /// </summary>
        internal static string FolderselectDescription {
            get {
                return ResourceManager.GetString("FolderselectDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Install tool to the PATH environment variable?.
        /// </summary>
        internal static string InstallToPathVar {
            get {
                return ResourceManager.GetString("InstallToPathVar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No folder was selected..
        /// </summary>
        internal static string NoFolderSelected {
            get {
                return ResourceManager.GetString("NoFolderSelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Question.
        /// </summary>
        internal static string Question {
            get {
                return ResourceManager.GetString("Question", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Recent Folders.
        /// </summary>
        internal static string RecentFolders {
            get {
                return ResourceManager.GetString("RecentFolders", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to remove the directory:\r\n {0} ?.
        /// </summary>
        internal static string RemoveFolder {
            get {
                return ResourceManager.GetString("RemoveFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to start shell. Application will exit..
        /// </summary>
        internal static string ShellScriptStartFail {
            get {
                return ResourceManager.GetString("ShellScriptStartFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to write start script. Application will exit..
        /// </summary>
        internal static string ShellScriptWriteFail {
            get {
                return ResourceManager.GetString("ShellScriptWriteFail", resourceCulture);
            }
        }
    }
}
