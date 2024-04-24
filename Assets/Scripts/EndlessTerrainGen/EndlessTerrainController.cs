using MeshGen;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrainController : MonoBehaviour
{
    [Range(300, 500)]
    [SerializeField]
    private float maxViewDistance = 300;
	[SerializeField]
	private Transform playerTransform;

    private int _chunkSize;
    private int _chunkVisibleInViewDistance;
	private Dictionary<Vector2, TerrainChunk> _terrainChunkDicitonary = new ();
	private Vector2 playerPositionVector2;
	private List<TerrainChunk> _terrainChunkVisibleLastUpdate = new ();

	private void Start()
	{
		_chunkSize = MeshDisplay.MAP_CHUNK_SIZE - 1;
		_chunkVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / _chunkSize);
		playerPositionVector2 = new Vector2();
	}

	private void FixedUpdate()
	{
		ConvertPlayerPositionToVector2();
		UpdateVisibleChunk();
	}

	private void ConvertPlayerPositionToVector2()
	{
		playerPositionVector2.x = playerTransform.position.x;
		playerPositionVector2.y = playerTransform.position.z;
	}

	private void UpdateVisibleChunk()
	{
		foreach (TerrainChunk terrainChunk in _terrainChunkVisibleLastUpdate)
		{
			terrainChunk.SetVisible(false);
		}

		_terrainChunkVisibleLastUpdate.Clear();


		int currentChunkCoordinateX = Mathf.RoundToInt(playerTransform.position.x / _chunkSize);
		int currentChunkCoordinateZ = Mathf.RoundToInt(playerTransform.position.z / _chunkSize);

		for (int zOffset = -_chunkVisibleInViewDistance; zOffset <= _chunkVisibleInViewDistance; zOffset++)
		{
			for (int xOffset = -_chunkVisibleInViewDistance; xOffset <= _chunkVisibleInViewDistance; xOffset++)
			{
				Vector2 viewChunkCoordinate = new Vector2(xOffset + currentChunkCoordinateX, zOffset + currentChunkCoordinateZ);

				if (!_terrainChunkDicitonary.ContainsKey(viewChunkCoordinate)){
					_terrainChunkDicitonary.Add(viewChunkCoordinate, new TerrainChunk(viewChunkCoordinate, _chunkSize, transform));
				}else{
					_terrainChunkDicitonary[viewChunkCoordinate].UpdateVisible(playerPositionVector2, maxViewDistance);
					if (_terrainChunkDicitonary[viewChunkCoordinate].IsVisible())
					{
						_terrainChunkVisibleLastUpdate.Add(_terrainChunkDicitonary[viewChunkCoordinate]);
					}
				}
			}
		}

	}

	public class TerrainChunk
	{
		private GameObject _meshObject;
		private Bounds _bounds;

		public TerrainChunk(Vector2 coordinate, int size, Transform parent)
		{
			coordinate *= size;
			Vector3 positionVector3 = new Vector3(coordinate.x, 0, coordinate.y);
			_bounds = new Bounds(coordinate, Vector2.one * size);
			_meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
			_meshObject.transform.position = positionVector3;
			_meshObject.transform.parent = parent;
			_meshObject.transform.localScale = (Vector3.one * size) / 10f; // Default Plane localScale is 10
			this.SetVisible(false);
		}

		public void SetVisible(bool visible) => _meshObject.SetActive(visible);

		public void UpdateVisible(Vector2 playerPosition, float maxViewDistance)
		{
			this.SetVisible(!IsOutViewDistance(playerPosition, maxViewDistance));
		}

		public bool IsOutViewDistance(Vector2 playerPosition, float maxViewDistance)
		{
			float sqrDistanceFromNearestEdgeToPlayer = _bounds.SqrDistance(playerPosition);
			return sqrDistanceFromNearestEdgeToPlayer > maxViewDistance * maxViewDistance;
		}

		public bool IsVisible() => _meshObject.activeSelf;


	}
}
