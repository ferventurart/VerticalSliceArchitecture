namespace Web.Api.Common.Abstractions;

public interface ICollectionResponse<T>
{
    List<T> Items { get; init; }
}
