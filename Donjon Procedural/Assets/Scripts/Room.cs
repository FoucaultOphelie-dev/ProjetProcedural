using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public bool isStartRoom = false;
	public Vector2Int position = Vector2Int.zero;

	private TilemapGroup _tilemapGroup;

	public static List<Room> allRooms = new List<Room>();

    private List<Color> initialsColor;
    private List<Color> initialsRendererColor;

    void Awake()
    {
		_tilemapGroup = GetComponentInChildren<TilemapGroup>();
		allRooms.Add(this);
        
	}

	private void OnDestroy()
	{
		allRooms.Remove(this);
	}

	void Start () {
        if(gameObject.GetComponent<RoomSettings>().type == Noeud.TYPE_DE_NOEUD.SECRET)
        {
            initialsColor = new List<Color>();
            foreach (STETilemap Tilemap in this.gameObject.GetComponentsInChildren<STETilemap>())
            {
                initialsColor.Add(Tilemap.TintColor);
                Tilemap.TintColor = new Color(0,0,0,0);
            }
            initialsRendererColor = new List<Color>();
            foreach(Renderer renderer in this.gameObject.GetComponentsInChildren<Renderer>())
            {
                initialsRendererColor.Add(renderer.material.color);
                renderer.material.color = new Color(0, 0, 0, 0);
            }
        }
        if(isStartRoom)
        {
            OnEnterRoom();
        }
    }
	
	public void OnEnterRoom()
    {
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        Bounds cameraBounds = _GetWorldRoomBounds();
        cameraFollow.SetBounds(cameraBounds);
		Player.Instance.EnterRoom(this);
        if (gameObject.GetComponent<RoomSettings>().type == Noeud.TYPE_DE_NOEUD.SECRET)
        {
            int i = 0;
            foreach (STETilemap Tilemap in this.gameObject.GetComponentsInChildren<STETilemap>())
            {
                Tilemap.TintColor = initialsColor[i];
                i++;
            }
            i = 0;
            foreach (Renderer renderer in this.gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = initialsRendererColor[i];
                i++;
            }
        }
    }


    private Bounds _GetLocalRoomBounds()
    {
		Bounds roomBounds = new Bounds(Vector3.zero, Vector3.zero);
		if (_tilemapGroup == null)
			return roomBounds;

		foreach (STETilemap tilemap in _tilemapGroup.Tilemaps)
		{
			Bounds bounds = tilemap.MapBounds;
			roomBounds.Encapsulate(bounds);
		}
		return roomBounds;
    }

    private Bounds _GetWorldRoomBounds()
    {
        Bounds result = _GetLocalRoomBounds();
        result.center += transform.position;
        return result;
    }

	public bool Contains(Vector3 position)
	{
		position.z = 0;
		return (_GetWorldRoomBounds().Contains(position));
	}
}
