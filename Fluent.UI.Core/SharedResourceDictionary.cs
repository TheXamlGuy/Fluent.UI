using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Fluent.UI.Core
{
    public class SharedResourceDictionary : ResourceDictionary
    {
        public static Dictionary<Uri, ResourceDictionary> _sharedDictionaries = new Dictionary<Uri, ResourceDictionary>();

        private Uri _sourceUri;

        private static object _lock = new object();

        public new Uri Source
        {
            get
            {
                if (IsInDesignMode)
                {
                    return base.Source;
                }

                return _sourceUri;
            }
            set
            {
                if (IsInDesignMode)
                {
                    try
                    {
                        _sourceUri = new Uri(value.OriginalString);
                    }
                    catch
                    {
                        // nothing
                    }

                    return;
                }

                try
                {
                    _sourceUri = new Uri(value.OriginalString);
                }
                catch
                {
                    // nothing
                }

                lock (_lock)
                {
                    if (!_sharedDictionaries.ContainsKey(value))
                    {
                        base.Source = value;
                        _sharedDictionaries.Add(value, this);
                    }
                    else
                    {
                        MergedDictionaries.Add(_sharedDictionaries[value]);
                    }
                }
   
            }
        }

        private static bool IsInDesignMode => (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue;
    }
}