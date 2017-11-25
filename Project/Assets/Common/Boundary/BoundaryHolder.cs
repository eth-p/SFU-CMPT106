/// <summary>
/// An interface for a class which has a BoundaryList.
/// </summary>
public interface BoundaryHolder {

	/// <summary>
	/// The boundary list.
	/// </summary>
	BoundaryList Boundaries { get; }
	
	/// <summary>
	/// The cached value of the tight boundary.
	/// </summary>
	BoundaryArea Tight { get; }
	
	/// <summary>
	/// The cached value of the loose boundary.
	/// </summary>
	BoundaryArea Loose { get; }

}
