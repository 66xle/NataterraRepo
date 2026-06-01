namespace Artifice
{
	public enum GizmoMode
	{
		None,
		Single,
		Range,
		Selection,
		Playing,
	}

	public enum GizmoValue
	{
		Sort,
		Element,
	}

	public enum CollisionMode
	{
		None,
		Simple,
		Raycast,
		//Full,
	}

	public enum PlaceMode
	{
		Time,
		Speed,
	}

	public enum BuildFromMode
	{
		None,
		Nearest,
		Random,
	}

	public enum BuildStyle
	{
		None,
		Appear,
		Radial,
		Vertical,
		Transform,
	}

	public enum BuildMode
	{
		None,
		Build,
		Dismantle,
		Click,
		Finished,
		Pause,
		Dismantled,
	}

	public enum SplitMode
	{
		Elements	= 1,	// Will find connected faces
		Materials	= 2,	// split just on material
		//UV1			= 4,	// Split using Uv
		//UV2			= 8,	// Split using uv2
		//Normal		= 16,	// Split using normals
		//Color		= 32,	// Split using color
	}

	public enum MeshPivotMode
	{
		Object,				// original mesh pivot
		Center,				// center of the mesh element
		Bottom,				// bottom of the mesh
		Top,				// top of the mesh
	}

	public enum SplineDir
	{
		Origin,
		SortOrigin,
	}

	public enum SortMode
	{
		None,
		Position,
		Material,
		PositionMaterial,
		MaterialPosition,
		Random,
	}

	public enum SortDistanceMode
	{
		Closest,
		Furthest,
		Centre,
	}

	public enum DismantleStyle
	{
		None,
		Vanish,
		Explode,
		Transform,
		Vertical,
		Radial,
	}

	public enum SplineMode
	{
		BuildFrom,
		MoveAlong,
	}

	public enum RotateMode
	{
		Normal,
		FlipXAxis,
		FlipYAxis,
		FlipZAxis,
	}

	[System.Flags]
	public enum LastElement
	{
		None	= 0,
		Last	= 1,
		LOD		= 2,
	}

	public enum StartMode
	{
		AsIs,				// Use as is from the editor
		StartBuilt,			// Start fully built
		StartDismantled,	// Start fully dismantled
		Build,				// start fully dismantled but building
		Dismantle,			// start fully assembled but dismantling
		ClickBuild,			// Start dismantled and in ClickBuild Mode
	}

	public enum AdjustMode
	{
		Selection,
		Volume,
	}

	public enum VolumeCheck
	{
		Center,
		Bounds,
	}

	public enum VolumeShape
	{
		Box,
		Sphere,
	}

	public enum AdjustShow
	{
		None,
		Bounds,
		Mesh,
	}

	public enum BoundsMode
	{
		Value,
		Calc,
	}

	public enum ClickBuildPos
	{
		Object,
		Part,
	}
}