using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Grid : MonoBehaviour
{
    public delegate void DeleteBlock();
    public event DeleteBlock onNotify;
    [SerializeField]
    private int _sizeX;
    [SerializeField]
    private int _sizeY;
    [SerializeField]
    private List<Block> _typeBlocks;
    [SerializeField]
    private Transform _startPosition;
    [SerializeField]
    private Block[,] _blocks;
    private Block _selectedBlock;
    [SerializeField]
    private bool _isSwipe;
    public bool IsSwipe { get; }
    [SerializeField]
    private TypeBlock _typeBlockCollect;

    Coroutine _swipeCoroutine;

    public void CreateGrid(TypeBlock typeBlock)
    {
        _typeBlockCollect = typeBlock;
        _blocks = new Block[_sizeY, _sizeX];
        for (int y = 0; y < _sizeY; y++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                var random = Random.Range(0, _typeBlocks.Count);
                Vector3 spawnPosition = new Vector3(_startPosition.position.x + 0.7f * x, _startPosition.position.y - 0.7f * y, _startPosition.position.z);
                var block = Instantiate(_typeBlocks[random], spawnPosition, Quaternion.identity);
                block.transform.parent = transform;
                block.SetY_X(y, x);
                _blocks[y, x] = block;
            }
        }
        CheckNearBlockForDelete();
    }

    private bool CheckNearBlockAround(Block block, bool isNotSwipe = false, bool isDebug = false)
    {
        var listRemove = new List<List<Block>>();
        var found = false;
        if (CheckNearBlock(block, Direction.UP, isDebug) != null) listRemove.Add(CheckNearBlock(block, Direction.UP));
        if (CheckNearBlock(block, Direction.DOWN, isDebug) != null) listRemove.Add(CheckNearBlock(block, Direction.DOWN));
        if (CheckNearBlock(block, Direction.LEFT, isDebug) != null) listRemove.Add(CheckNearBlock(block, Direction.LEFT));
        if (CheckNearBlock(block, Direction.RIGHT, isDebug) != null) listRemove.Add(CheckNearBlock(block, Direction.RIGHT));
        if (listRemove[0].Count + listRemove[1].Count >= 2)
        {
            for (int i = 0; i < listRemove[0].Count; i++)
            {
                var tmpBlock = listRemove[0][i];
                _blocks[tmpBlock.Y, tmpBlock.X] = null;
                Destroy(listRemove[0][i].gameObject);
                // listRemove[0][i].GetComponent<SpriteRenderer>().color = Color.black;
            }
            for (int i = 0; i < listRemove[1].Count; i++)
            {
                var tmpBlock = listRemove[1][i];
                _blocks[tmpBlock.Y, tmpBlock.X] = null;
                Destroy(listRemove[1][i].gameObject);
                // listRemove[1][i].GetComponent<SpriteRenderer>().color = Color.black;
            }
            found = true;
        }
        if (listRemove[2].Count + listRemove[3].Count >= 2)
        {
            for (int i = 0; i < listRemove[2].Count; i++)
            {
                var tmpBlock = listRemove[2][i];
                _blocks[tmpBlock.Y, tmpBlock.X] = null;
                Destroy(listRemove[2][i].gameObject);
                // listRemove[2][i].GetComponent<SpriteRenderer>().color = Color.black;
            }
            for (int i = 0; i < listRemove[3].Count; i++)
            {
                var tmpBlock = listRemove[3][i];
                _blocks[tmpBlock.Y, tmpBlock.X] = null;
                Destroy(listRemove[3][i].gameObject);
                // listRemove[3][i].GetComponent<SpriteRenderer>().color = Color.black;
            }
            found = true;
            // Destroy(block1.gameObject);
        }
        if (found)
        {
            if (block.TypeBlock == _typeBlockCollect && !isNotSwipe)
            {
                onNotify?.Invoke();
            }
            else
            {
                // Debug.Log("Not update");
            }
            _blocks[block.Y, block.X] = null;
            Destroy(block.gameObject);
        }
        listRemove.Clear();
        return found;
    }

    private List<Block> CheckNearBlock(Block current, Direction direction, bool isDebug = false)
    {
        var list = new List<Block>();
        if (isDebug)
        {
            Debug.Log(current.ShowXY());
            Debug.Log(current.TypeBlock);
        }
        switch (direction)
        {
            case Direction.DOWN:
                if (current.Y + 1 < _sizeY && CheckTypeBlock(_blocks[current.Y + 1, current.X], current))
                {
                    list.Add(_blocks[current.Y + 1, current.X]);
                    if (current.Y + 2 < _sizeY && CheckTypeBlock(_blocks[current.Y + 2, current.X], current))
                    {
                        list.Add(_blocks[current.Y + 2, current.X]);
                    }
                }
                break;
            case Direction.UP:
                if (current.Y - 1 >= 0 && CheckTypeBlock(_blocks[current.Y - 1, current.X], current))
                {
                    list.Add(_blocks[current.Y - 1, current.X]);
                    if (current.Y - 2 >= 0 && CheckTypeBlock(_blocks[current.Y - 2, current.X], current))
                    {
                        list.Add(_blocks[current.Y - 2, current.X]);
                    }
                }
                break;
            case Direction.RIGHT:
                if (current.X + 1 < _sizeX && CheckTypeBlock(_blocks[current.Y, current.X + 1], current))
                {
                    list.Add(_blocks[current.Y, current.X + 1]);
                    if (current.X + 2 < _sizeX && CheckTypeBlock(_blocks[current.Y, current.X + 2], current))
                    {
                        list.Add(_blocks[current.Y, current.X + 2]);
                    }
                }
                break;
            case Direction.LEFT:
                if (current.X - 1 >= 0 && CheckTypeBlock(_blocks[current.Y, current.X - 1], current))
                {
                    list.Add(_blocks[current.Y, current.X - 1]);
                    if (current.X - 2 >= 0 && CheckTypeBlock(_blocks[current.Y, current.X - 2], current))
                    {
                        list.Add(_blocks[current.Y, current.X - 2]);
                    }
                }
                break;
            default:
                return list;
        }
        if (isDebug)
        {
            Debug.Log(direction + " " + list.Count);
            var strTmp = "";
            foreach (var block in list)
            {
                strTmp += block.name + " ";
            }
            Debug.Log(strTmp);
        }

        return list;
    }

    private void UpdateGrid()
    {
        bool isUpdate = false;
        for (int y = 0; y < _sizeY; y++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                if (_blocks[y, x] == null)
                {
                    isUpdate = true;
                    var random = Random.Range(0, _typeBlocks.Count);
                    Vector3 spawnPosition = new Vector3(_startPosition.position.x + 0.7f * x, _startPosition.position.y - 0.7f * y, _startPosition.position.z);
                    var block = Instantiate(_typeBlocks[random], spawnPosition, Quaternion.identity);
                    block.transform.parent = transform;
                    block.SetY_X(y, x);
                    _blocks[y, x] = block;
                }
            }
        }
        if (isUpdate) CheckNearBlockForDelete();
    }

    private void CheckNearBlockForDelete()
    {
        for (int y = 0; y < _sizeY; y++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                var block = _blocks[y, x];
                if (block)
                    CheckNearBlockAround(block, true);
            }
        }
        UpdateGrid();
    }

    private bool CheckTypeBlock(Block block1, Block block2)
    {
        if (!block1) return false;
        return block1.TypeBlock.Equals(block2.TypeBlock);
    }

    public IEnumerator SwipeBlock(Block block1, Block block2)
    {
        var tmpPos1 = block1.transform.position;
        var tmpPos2 = block2.transform.position;
        float deltaTime = 0;
        float timeDurtaion = 1;
        _isSwipe = true;
        while (true)
        {
            deltaTime += Time.deltaTime;
            block1.transform.position = Vector3.Lerp(block1.transform.position, tmpPos2, deltaTime / timeDurtaion);
            block2.transform.position = Vector3.Lerp(block2.transform.position, tmpPos1, deltaTime / timeDurtaion);
            yield return null;//new WaitForSeconds(.1f);
            if (deltaTime / timeDurtaion >= 1)
            {
                _isSwipe = false;
                break;
            }
        }
        block1.GetComponent<BoxCollider2D>().enabled = true;
        block2.GetComponent<BoxCollider2D>().enabled = true;
        _blocks[block1.Y, block1.X] = block2;
        _blocks[block2.Y, block2.X] = block1;
        int tmpX = block1.X;
        int tmpY = block1.Y;
        block1.SetY_X(block2);
        block2.SetY_X(tmpY, tmpX);

        if (CheckNearBlockAround(block1) || CheckNearBlockAround(block2)) UpdateGrid();
        // _isSwipe = false;
    }
}

public enum Direction
{
    UP,
    DOWN,
    RIGHT,
    LEFT
}
