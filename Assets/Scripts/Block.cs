using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Block : MonoBehaviour
{
    [SerializeField]
    private TypeBlock _typeBlock;
    public TypeBlock TypeBlock { get { return _typeBlock; } }
    [SerializeField]
    private int _posY;
    [SerializeField]
    private int _posX;

    public int Y { get { return _posY; } }
    public int X { get { return _posX; } }

    public void SetY_X(int y, int x)
    {
        var sb = new StringBuilder();
        _posX = x;
        _posY = y;
        gameObject.name = sb.AppendFormat("Place {0} {1}", y, x).ToString();
    }

    public void SetY_X(Block block)
    {
        SetY_X(block.Y, block.X);
    }

    public string ShowXY()
    {
        return transform.gameObject.name + " x : " + _posX + " y: " + _posY;
    }

    // IEnumerator CheckDown()
    // {
    //     while (true)
    //     {
    //         // RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1);
    //         RaycastHit2D[] hits = new RaycastHit2D[2];
    //         Physics2D.RaycastNonAlloc(transform.position, -Vector2.up, hits, 0.5f);
    //         // Debug.DrawRay(transform.position, hit.point, Color.black, 0.4f);
    //         if (hits[1] == null || hits[1].collider == null)
    //         {
    //             transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    //             _grounded = false;
    //             _objectDown = null;
    //         }
    //         else
    //         {
    //             _grounded = true;
    //             _objectDown = hits[1].collider.gameObject;
    //             transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    //         }
    //         yield return new WaitForSeconds(.5f);
    //     }
    // }
}

public enum TypeBlock
{
    StarDark,
    Bucket,
    Origami,
    Ball,
    StarLight,
    Racket
}
