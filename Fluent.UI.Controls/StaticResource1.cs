using System;
using System.Collections.Generic;
using System.Text;

namespace Fluent.UI.Controls
{
    public class StaticResource1Extension : System.Windows.StaticResourceExtension
    {
        public StaticResource1Extension()

        {

        }



        /// <summary>

        ///  Constructor that takes the resource key that this is a static reference to.

        /// </summary>

        public StaticResource1Extension(

            object resourceKey)

        {

            if (resourceKey == null)

            {

                throw new ArgumentNullException("resourceKey");

            }

           ResourceKey = resourceKey;

        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return base.ProvideValue(serviceProvider);
        }
    }
}
