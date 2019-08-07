namespace Fluent.UI.Core
{
    public delegate void TypedEventHandler<in TSender, in TResult>(TSender sender, TResult args);
}
