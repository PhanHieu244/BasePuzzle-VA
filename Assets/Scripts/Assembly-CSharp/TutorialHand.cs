using UnityEngine;

public class TutorialHand : MonoBehaviour
{
	private void Update()
	{
		if (MainController.instance.level > 1 || MainController.instance.world > 1)
		{
			base.gameObject.SetActive(false);
		}
		if (TileRegion.instance.pieces.Count <= 0)
		{
			return;
		}
		foreach (Piece piece in TileRegion.instance.pieces)
		{
			if (piece.id == 0 && (piece.status == Piece.Status.Dragging || piece.status == Piece.Status.OnBoard))
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
