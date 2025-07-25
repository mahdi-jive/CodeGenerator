namespace CodeGenerator.Abstractions
{
    public interface IDirectoryMarker<TParent> : IDirectoryMarkerParent
    where TParent : IDirectoryMarkerParent
    {

    }
}
