using UnityEngine;

public class CMonoUtils : MonoBehaviour
{
	public Tile tile_background;

	public Tile tile_background2;

	public Tile tile_background_bottom;

	public Tile tile_shadow;

	public Tile tile_stone;

	public Tile tile_normal;

	public Piece piecePrefab;

	public Piece2 piece2Prefab;

	public Transform dragRegion;

	public Transform bottomRegion;

	public Transform pieceRegion;

	public Sprite[] tileSprites;

	public Transform hintPiecesTransform;

	public Transform backgroundTilesTransform;

	public Transform piecesTransform;

	public Transform piecesBottomTransform;

	public Transform bottomRegionBGTransform;

	public Transform highlightsTransform;

	public static CMonoUtils instance;

	private void Awake()
	{
		instance = this;
	}
}
