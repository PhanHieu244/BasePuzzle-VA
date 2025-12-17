using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
	private Sprite _sprZero;

	public void ShowUnhideableWindow(string message)
	{
		Canvas canvas = CreateCanvas();
		Image image = CreateRect(canvas.transform).gameObject.AddComponent<Image>();
		image.sprite = spriteZero();
		image.color = new Color(0f, 0f, 0f, 0.75f);
		Image image2 = CreateRect(canvas.transform, 500, 300).gameObject.AddComponent<Image>();
		Text text = CreateRect(image2.transform, 500, 300).gameObject.AddComponent<Text>();
		text.fontSize = 40;
		text.alignment = TextAnchor.MiddleCenter;
		text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		text.color = Color.black;
		text.text = message;
	}

	public RectTransform CreateRect(Transform parent = null, int w = -1, int h = -1)
	{
		Transform transform = new GameObject().transform;
		transform.parent = parent;
		if (w == -1 && h == -1)
		{
			w = Camera.main.pixelWidth;
			h = Camera.main.pixelHeight;
		}
		RectTransform rectTransform = transform.gameObject.AddComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(w, h);
		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		return rectTransform;
	}

	public Sprite spriteZero()
	{
		if (_sprZero == null)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, new Color(1f, 1f, 1f, 1f));
			texture2D.Apply();
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 40f);
		}
		return _sprZero;
	}

	public Canvas CreateCanvas()
	{
		Canvas canvas = new GameObject().AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.worldCamera = Camera.main;
		canvas.sortingOrder = 90;
		CanvasScaler canvasScaler = canvas.gameObject.AddComponent<CanvasScaler>();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		canvasScaler.referenceResolution = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
		canvasScaler.matchWidthOrHeight = 0f;
		return canvas;
	}
}
