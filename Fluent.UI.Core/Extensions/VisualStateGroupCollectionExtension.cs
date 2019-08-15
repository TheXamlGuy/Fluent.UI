using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

namespace Fluent.UI.Core.Extensions
{
    public static class VisualStateGroupCollectionExtension
    {
        public static IEnumerable<object> FindKeyFrames(this IEnumerable<VisualStateGroup> visualStateGroups)
        {
            foreach (var timeline in visualStateGroups.Select(vsg => vsg.States.Cast<VisualState>().Where(x => x.Storyboard != null)).SelectMany(visualStates => visualStates.SelectMany(sb => sb.Storyboard.Children)))
            {
                if (timeline is IKeyFrameAnimation keyFrameAnimation)
                {
                    foreach (var keyFrame in keyFrameAnimation.KeyFrames)
                    {
                        yield return keyFrame;
                    }
                }
            }
        }

    }
}
