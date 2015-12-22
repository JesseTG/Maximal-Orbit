using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[DisallowMultipleComponent]
public class DragRendering : MonoBehaviour
{
    private LineRenderer _line;
    private int _originID;
    private GameManager _gameManager;
    private PlanetManager _planetManager;

    void Start()
    {
        this._line = GetComponent<LineRenderer>();
        this._originID = Shader.PropertyToID("_Origin");
        this._gameManager = GameObject.FindObjectOfType<GameManager>();
        this._planetManager = GameObject.FindObjectOfType<PlanetManager>();
    }

    void OnEnable()
    {
        if (this._line)
        {
            this._line.enabled = true;
            Vector3 dropOff = _planetManager.PlacingDropOff;
            _line.SetPosition(0, dropOff);
            _line.SetPosition(1, dropOff);
        }
    }

    void OnDisable()
    {
        this._line.enabled = false;
    }

    void Update()
    {
        if (_planetManager.State == PlacingState.Placing)
        {
            Vector3 dropOff = _planetManager.PlacingDropOff;
            if (Input.GetButtonDown("PlacePlanet"))
            {
                _line.SetPosition(0, dropOff);
                _line.SetPosition(1, dropOff);
            }

            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0;
            Vector3 diff = Vector3.ClampMagnitude(dropOff - mouse, _planetManager.MaxDistance);
            Vector3 pos = dropOff - diff;
            pos.z = 0;
            _line.SetPosition(1, pos);
            _line.sharedMaterial.SetVector(_originID, Camera.main.WorldToScreenPoint(pos));
        }

    }
}
