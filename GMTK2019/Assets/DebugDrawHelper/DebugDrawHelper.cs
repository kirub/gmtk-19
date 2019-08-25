using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawHelper : MonoBehaviour
{
	// -------------------------------------------------------------------------------------

	private static DebugDrawHelper Instance = null;
	
	public static void RegisterDrawable(GameObject BaseObject, IDebugDrawable DrawableComponent)
	{
		if (!Instance)
			return;

#if UNITY_EDITOR
		if (!Instance.DrawablesList.Contains(BaseObject))
		{
			Instance.DrawablesList.Add(BaseObject);
		}

		if (!Instance.Drawables.ContainsKey(BaseObject))
		{
			Instance.Drawables.Add(BaseObject, new List<IDebugDrawable>());
		}

		List<IDebugDrawable> Drawables = Instance.Drawables[BaseObject];
		if (!Drawables.Contains(DrawableComponent))
		{
			Drawables.Add(DrawableComponent);
		}
#endif // UNITY_EDITOR
	}

	public static void UnregisterDrawable(GameObject BaseObject, IDebugDrawable DrawableComponent)
	{
		if (!Instance)
			return;

#if UNITY_EDITOR
		if (Instance.Drawables.ContainsKey(BaseObject))
		{
			Instance.Drawables[BaseObject].Remove(DrawableComponent);
			if (Instance.Drawables[BaseObject].Count == 0)
			{
				Instance.Drawables.Remove(BaseObject);
				Instance.DrawablesList.Remove(BaseObject);
			}
		}
#endif // UNITY_EDITOR
	}

	// -------------------------------------------------------------------------------------

	[SerializeField] private KeyCode ToggleDebugDrawKey = KeyCode.F1;
	[SerializeField] private KeyCode ToggleDisplayDrawablesListKey = KeyCode.F2;
	[SerializeField] private KeyCode SwitchDisplayTypeKey = KeyCode.F3;
	[SerializeField] private KeyCode DisplayShortcutsKey = KeyCode.F4;
	[SerializeField] private KeyCode PrevItemKey = KeyCode.UpArrow;
	[SerializeField] private KeyCode NextItemKey = KeyCode.DownArrow;
	[SerializeField] private KeyCode PrevPageKey = KeyCode.LeftArrow;
	[SerializeField] private KeyCode NextPageKey = KeyCode.RightArrow;
	[SerializeField] private KeyCode ToggleSelectionKey = KeyCode.Return;

	[SerializeField] private Color DefaultColor = Color.black;
	[SerializeField] private Color SelectedColor = Color.green;
	[SerializeField] private Color HighlightedColor = Color.blue;
	[SerializeField] private Color HighlightedSelectedColor = Color.cyan;

	[SerializeField] private int MaxDrawablesPerPage = 10;

	private enum EDisplayType
	{
		All,
		None,
		Selected,
		MAX
	};

	private Dictionary<GameObject, List<IDebugDrawable>> Drawables = new Dictionary<GameObject, List<IDebugDrawable>>();
	private List<GameObject> DrawablesList = new List<GameObject>();

	private List<bool> SelectedToggled = new List<bool>();
	private List<GameObject> SelectedDrawables = new List<GameObject>();

	private bool IsActive = false;
	private bool IsListDisplayed = false;
	private bool IsShortcutsDisplayed = false;
	private EDisplayType DisplayType = EDisplayType.All;

	private GUIStyle Style = new GUIStyle();

	private int CurrentDisplayedPage = 0;
	private int CurrentHighlightedItem = 0;

	public int StartDrawable { get { return CurrentDisplayedPage * MaxDrawablesPerPage; } }
	public int EndDrawable { get { return (CurrentDisplayedPage + 1) * MaxDrawablesPerPage; } }
	public int CurrentHighlightedItemIndex { get { return StartDrawable + CurrentHighlightedItem; } }

	public int MaxFullPages { get { return Mathf.CeilToInt( DrawablesList.Count / (float)MaxDrawablesPerPage ); } }
	public int MaxSelectedPages { get { return Mathf.CeilToInt( MaxSelectedItems / (float)MaxDrawablesPerPage ); } }
	public int MaxSelectedItems
	{
		get
		{
			switch (DisplayType)
			{
				case EDisplayType.All:
					return DrawablesList.Count;
				case EDisplayType.None:
					return 0;
				case EDisplayType.Selected:
				default:
					return SelectedDrawables.Count;
			}
		}
	}

	public List<GameObject> SelectedList
	{
		get
		{
			switch (DisplayType)
			{
				case EDisplayType.All:
					return DrawablesList;
				case EDisplayType.None:
					return new List<GameObject>();
				case EDisplayType.Selected:
				default:
					return SelectedDrawables;
			}
		}
	}

	private void Awake()
	{
		if (Instance && Instance != this)
		{
			Debug.LogError("Multiple instances of DebugDrawHelper!");
			Destroy(this);
			return;
		}
		Instance = this;
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (!UpdateIsActive())
		{
			return;
		}

		if (SelectedToggled.Count != SelectedList.Count)
		{
			RefreshSelectedToggled();
		}

		UpdateShortcuts();

		if (IsListDisplayed)
		{
			UpdateDisplayDrawablesList();
		}
		else
		{
			UpdateDisplayDrawables();
		}
#endif // UNITY_EDITOR
	}

	private void OnGUI()
	{
#if UNITY_EDITOR
		const float Inc = 20f;
		Rect Pos = new Rect(10, 10, Screen.width - 10, Screen.height - 10);

		if (!IsActive)
		{
			Style.fontStyle = FontStyle.Bold;

			GUI.Label(Pos, "- Toggle Debug Draw " + ToggleDebugDrawKey, Style);
			Pos.y += Inc;

			Style.fontStyle = FontStyle.Normal;
			return;
		}

		Style.normal.textColor = DefaultColor;

		if (IsShortcutsDisplayed)
		{
			OnGUIShortcuts(Pos, Inc);
		}
		else
		{
			GUI.Label(Pos, "Display Shortcuts " + DisplayShortcutsKey, Style);
			Pos.y += Inc;

			if (IsListDisplayed)
			{
				OnGUIListDrawables(Pos, Inc);
			}
			else
			{
				OnGUISelectedDrawables(Pos, Inc);
			}
		}
#endif // UNITY_EDITOR
	}

	// -------------------------------------------------------------------------------------

	Color GetColorFromStatus(bool IsSelected, bool IsHighlighted)
	{
		if (IsSelected)
		{
			if (IsHighlighted)
			{
				return HighlightedSelectedColor;
			}
			else
			{
				return SelectedColor;
			}
		}
		else
		{
			if (IsHighlighted)
			{
				return HighlightedColor;
			}
			else
			{
				return DefaultColor;
			}
		}
	}

	void OnGUIShortcuts(Rect Pos, float Inc)
	{
		Style.fontStyle = FontStyle.Bold;

		GUI.Label(Pos, "Shortcuts", Style);
		Pos.y += Inc;
		Pos.y += Inc;

		Style.fontStyle = FontStyle.Normal;

		GUI.Label(Pos, "- Toggle Debug Draw " + ToggleDebugDrawKey, Style);
		Pos.y += Inc;
		GUI.Label(Pos, "- Toggle Display Drawables List " + ToggleDisplayDrawablesListKey, Style);
		Pos.y += Inc;
		GUI.Label(Pos, "- Switch Display Type " + SwitchDisplayTypeKey, Style);
		Pos.y += Inc;
		GUI.Label(Pos, "- Display Shortcuts " + DisplayShortcutsKey, Style);
		Pos.y += Inc;
		GUI.Label(Pos, "- Previous Item " + PrevItemKey, Style);
		Pos.y += Inc;
		GUI.Label(Pos, "- Next Item " + NextItemKey, Style);
		Pos.y += Inc;
		GUI.Label(Pos, "- Previous Page " + PrevPageKey, Style);
		Pos.y += Inc;
		GUI.Label(Pos, "- Next Page " + NextPageKey, Style);
		Pos.y += Inc;
		GUI.Label(Pos, "- Toggle Selection " + ToggleSelectionKey, Style);
		Pos.y += Inc;
	}

	void OnGUIDisplayHighlighted( GameObject HighlightedObject, bool IsSelected )
	{
		Color BaseColor = Style.normal.textColor;
		if (HighlightedObject)
		{
			const float Width = 500f;
			const float Height = 20f;

			Vector3 ScreenPos = Camera.main.WorldToScreenPoint(HighlightedObject.transform.position);
			Rect Pos = new Rect(ScreenPos.x, Screen.height - ScreenPos.y, Width, Height);

			Style.normal.textColor = GetColorFromStatus(IsSelected, true);
			GUI.Label(Pos, "+ " + HighlightedObject.name, Style);
		}

		Style.normal.textColor = BaseColor;
	}

	void OnGUIListDrawables(Rect Pos, float Inc)
	{
		Style.fontStyle = FontStyle.Bold;

		GUI.Label(Pos, "Select drawables to display", Style);
		Pos.y += Inc;
		Pos.y += Inc;

		Style.fontStyle = FontStyle.Normal;

		if (DrawablesList.Count == 0)
		{
			GUI.Label(Pos, "No Drawables registered.", Style);
			Pos.y += Inc;
		}
		else
		{
			for (int i = StartDrawable; i < EndDrawable && i < DrawablesList.Count; ++i)
			{
				GameObject CurrentObj = DrawablesList[i];
				if (CurrentObj)
				{
					bool IsSelected = SelectedDrawables.Contains(CurrentObj);
					bool IsHighlighted = CurrentHighlightedItemIndex == i;
					string SelectionStr = IsSelected ? "X" : " ";
					Style.normal.textColor = GetColorFromStatus(IsSelected, IsHighlighted);

					GUI.Label(Pos, "[" + SelectionStr + "] " + CurrentObj.name, Style);
					Pos.y += Inc;

					if (IsHighlighted)
					{
						OnGUIDisplayHighlighted(CurrentObj, IsSelected);
					}
				}
				else
				{
					GUI.Label(Pos, "[ ] NULL", Style);
					Pos.y += Inc;
				}
			}

			if (MaxFullPages > 1)
			{
				Pos.y += Inc;
				GUI.Label(Pos, "Page " + (CurrentDisplayedPage + 1) + "/" + MaxFullPages, Style);
				Pos.y += Inc;
			}
		}
	}

	void OnGUISelectedDrawables(Rect Pos, float Inc)
	{
		List<GameObject> ListToUse = SelectedList;

		Style.fontStyle = FontStyle.Bold;
		
		GUI.Label(Pos, "Drawables displayed displayed : " + DisplayType, Style);
		Pos.y += Inc;
		Pos.y += Inc;

		Style.fontStyle = FontStyle.Normal;

		if (ListToUse.Count == 0)
		{
			GUI.Label(Pos, "No Drawables to display.", Style);
			Pos.y += Inc;
		}
		else
		{
			for (int i = StartDrawable; i < EndDrawable && i < ListToUse.Count; ++i)
			{
				GameObject CurrentObj = ListToUse[i];
				if (CurrentObj)
				{
					bool IsSelected = SelectedToggled[i];
					bool IsHighlighted = CurrentHighlightedItemIndex == i;
					string SelectionStr = IsSelected ? "v" : ">";
					Style.normal.textColor = GetColorFromStatus(IsSelected, IsHighlighted);

					GUI.Label(Pos, SelectionStr + " " + CurrentObj.name, Style);
					Pos.y += Inc;

					if (IsHighlighted)
					{
						OnGUIDisplayHighlighted(CurrentObj, IsSelected);
					}

					Style.normal.textColor = DefaultColor;

					if (IsSelected)
					{
						Pos.x += Inc;

						List<IDebugDrawable> ToDraw = Drawables[CurrentObj];
						foreach (IDebugDrawable CurDrawable in ToDraw)
						{
							if (CurDrawable != null)
							{
								CurDrawable.DebugDraw(ref Pos, Inc, Style);
							}
						}

						Pos.x -= Inc;
					}
				}
				else
				{
					GUI.Label(Pos, "- NULL", Style);
					Pos.y += Inc;
				}
			}

			if (MaxSelectedPages > 1)
			{
				Pos.y += Inc;
				GUI.Label(Pos, "Page " + (CurrentDisplayedPage + 1) + "/" + MaxSelectedPages, Style);
				Pos.y += Inc;
			}
		}
	}

	// -------------------------------------------------------------------------------------

	bool UpdateIsActive()
	{
		if (Input.GetKeyDown(ToggleDebugDrawKey))
		{
			IsActive = !IsActive;
		}

		return IsActive;
	}

	void UpdateShortcuts()
	{
		if (Input.GetKeyDown(ToggleDisplayDrawablesListKey))
		{
			IsListDisplayed = !IsListDisplayed;
			CurrentDisplayedPage = 0;
			CurrentHighlightedItem = 0;
		}

		if (Input.GetKeyDown(SwitchDisplayTypeKey))
		{
			DisplayType += 1;
			if (DisplayType == EDisplayType.MAX)
			{
				DisplayType = 0;
			}
			RefreshSelectedToggled();
		}

		IsShortcutsDisplayed = Input.GetKey(DisplayShortcutsKey);
	}

	void UpdateDisplayDrawablesList()
	{
		UpdatePage(MaxFullPages);
		UpdateHighlightedItem(DrawablesList.Count, MaxFullPages);

		if (Input.GetKeyDown(ToggleSelectionKey))
		{
			GameObject SelectedGO = DrawablesList[CurrentHighlightedItemIndex];
			if (SelectedDrawables.Contains(SelectedGO))
			{
				SelectedDrawables.Remove(SelectedGO);
			}
			else
			{
				SelectedDrawables.Add(SelectedGO);
			}
		}
	}

	void UpdateDisplayDrawables()
	{
		UpdatePage(MaxSelectedPages);
		UpdateHighlightedItem(MaxSelectedItems, MaxSelectedPages);

		if (Input.GetKeyDown(ToggleSelectionKey))
		{
			SelectedToggled[CurrentHighlightedItemIndex] = !SelectedToggled[CurrentHighlightedItemIndex];
		}
	}

	// -------------------------------------------------------------------------------------

	void UpdateHighlightedItem(int TotalItems, int MaxPage)
	{
		int NumItemsOnMaxPage = TotalItems - (MaxPage - 1) * MaxDrawablesPerPage;
		int MaxPossibleItem = CurrentDisplayedPage == MaxPage - 1 ? NumItemsOnMaxPage : MaxDrawablesPerPage;

		if (Input.GetKeyDown(PrevItemKey))
		{
			--CurrentHighlightedItem;
			if (CurrentHighlightedItem < 0)
			{
				CurrentHighlightedItem = MaxPossibleItem - 1;
			}
		}
		else if (Input.GetKeyDown(NextItemKey))
		{
			++CurrentHighlightedItem;
		}

		if (CurrentHighlightedItem >= MaxPossibleItem)
		{
			CurrentHighlightedItem = 0;
		}
	}

	void UpdatePage(int MaxPage )
	{
		if ( Input.GetKeyDown(PrevPageKey) )
		{
			--CurrentDisplayedPage;
			if (CurrentDisplayedPage < 0)
			{
				CurrentDisplayedPage = MaxPage - 1;
				CurrentHighlightedItem = 0;
			}
		}
		else if(Input.GetKeyDown(NextPageKey))
		{
			++CurrentDisplayedPage;
			if (CurrentDisplayedPage >= MaxPage)
			{
				CurrentDisplayedPage = 0;
				CurrentHighlightedItem = 0;
			}
		}
	}

	void RefreshSelectedToggled()
	{
		SelectedToggled = new List<bool>(DrawablesList.Count);
		List<GameObject> ListToUse = SelectedList;
		for (int i = 0; i < ListToUse.Count; ++i)
		{
			SelectedToggled.Add(false);
		}
	}
}
