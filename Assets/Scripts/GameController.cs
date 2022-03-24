using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    private struct SpriteTypeBlock
    {
        public TypeBlock typeBlock;
        public Sprite sprite;
    }

    [SerializeField]
    private Text _textMovesCount;
    [SerializeField]
    private Text _textCollect;
    [SerializeField]
    private Image _spriteCollect;
    [SerializeField]
    private Background _background;
    [SerializeField]
    private Grid _grid;
    [SerializeField]
    private bool _isSwipe;
    [SerializeField]
    private Block _selectedBlock;
    [SerializeField]
    private List<Sprite> _sprites;
    [SerializeField]
    private int _countMoves;
    [SerializeField]
    private int _countCollect;
    [SerializeField]
    private TypeBlock _collectTypeBlock;
    [SerializeField]
    private List<SpriteTypeBlock> _spriteTypeBlocks;
    [SerializeField]
    private GameObject _win;
    [SerializeField]
    private GameObject _lose;

    private void Start()
    {
        _countCollect = 2;
        foreach (var tmp in _spriteTypeBlocks)
        {
            if (tmp.typeBlock == StaticDate.TypeBlock)
            {
                _spriteCollect.sprite = tmp.sprite;
            }
        }
        _collectTypeBlock = StaticDate.TypeBlock;
        _countCollect = StaticDate.CurrentLevel;
        _countCollect = StaticDate.CountCollect;
        _countMoves = StaticDate.CountMoves;
        Debug.Log(_countCollect);
        var random = Random.Range(0, _sprites.Count);
        _background.SetImage(_sprites[random]);
        _grid.CreateGrid(_collectTypeBlock);
        _textMovesCount.text = "Moves " + _countMoves.ToString();
        _textCollect.text = _countCollect.ToString();
    }

    private void OnEnable()
    {
        _grid.onNotify += UpdateCollect;
    }

    private void OnDisable()
    {
        _grid.onNotify -= UpdateCollect;
    }

    private void UpdateCollect()
    {
        _countCollect--;
        _textCollect.text = _countCollect.ToString();
    }

    private void Update()
    {
        if (_countMoves > 0 && _countCollect > 0)
        {
            TouchHandler();
        }
        if (_countCollect == 0)
        {
            _lose.SetActive(false);
            if (!StaticDate.LevelOpen.Contains(StaticDate.CurrentLevel))
            {
                StaticDate.LevelOpen.Add(StaticDate.CurrentLevel);
                Debug.Log("New Level Open");
            }
            _win.SetActive(true);
        }
        if (_countMoves == 0 && _countCollect != 0)
        {
            // Debug.Log("Lose");
            _lose.SetActive(true);
        }
    }

    private void TouchHandler()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !_isSwipe)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {
                Block block = hit.transform.gameObject.GetComponent<Block>();
                if (!block && !_grid.IsSwipe) return;
                if (_selectedBlock && hit.collider.name != _selectedBlock.name)
                {
                    var distance = System.Math.Round(Vector2.Distance(_selectedBlock.transform.position, block.transform.position), 2);
                    if (distance > 0.7)
                    {
                        _selectedBlock.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        _selectedBlock.GetComponent<SpriteRenderer>().color = Color.white;
                        StartCoroutine(_grid.SwipeBlock(_selectedBlock, block));
                        _countMoves--;
                        _textMovesCount.text = "Moves " + _countMoves.ToString();
                    }
                }
                _selectedBlock = block;
                block.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
    }

    public void MenuLoad()
    {
        SceneManager.LoadScene("ChoiceLevel");
    }

    public void Restart()
    {
        if (StaticDate.CountCollect > 0) _countCollect = StaticDate.CountCollect;
        if (StaticDate.CountMoves > 0) _countMoves = StaticDate.CountMoves;
        _textMovesCount.text = "Moves " + _countMoves.ToString();
        _textCollect.text = _countCollect.ToString();
        _lose.SetActive(false);
    }

    public void NextLevel()
    {
        StaticDate.isNextLevel = true;
        SceneManager.LoadScene("ChoiceLevel");
    }
}
