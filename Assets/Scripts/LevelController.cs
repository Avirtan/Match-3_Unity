using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private List<Button> _levels;
    [SerializeField]
    private List<Sprite> _spriteOpenLevel;
    [SerializeField]
    private Sprite _levelOpenSprite;

    void Start()
    {
        foreach (var level in StaticDate.LevelOpen)
        {
            _levels[level].enabled = true;
            _levels[level].image.sprite = _spriteOpenLevel[level];
        }
        if (StaticDate.isNextLevel)
        {
            StaticDate.isNextLevel = false;
            switch (StaticDate.CurrentLevel)
            {
                case 1:
                    LoadLevel2();
                    break;
                case 2:
                    LoadLevel3();
                    break;
                case 3:
                    LoadLevel4();
                    break;
                case 4:
                    LoadLevel5();
                    break;
                case 5:
                    LoadLevel6();
                    break;
                case 6:
                    LoadLevel7();
                    break;
                case 7:
                    LoadLevel8();
                    break;
            }
        }
    }

    public void LoadLevel1()
    {
        StaticDate.CurrentLevel = 1;
        StaticDate.CountMoves = 20;
        StaticDate.CountCollect = 1;
        StaticDate.TypeBlock = TypeBlock.StarLight;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadLevel2()
    {
        StaticDate.CurrentLevel = 2;
        StaticDate.CountMoves = 20;
        StaticDate.CountCollect = 2;
        StaticDate.TypeBlock = TypeBlock.Ball;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadLevel3()
    {
        StaticDate.CurrentLevel = 3;
        StaticDate.CountMoves = 30;
        StaticDate.CountCollect = 3;
        StaticDate.TypeBlock = TypeBlock.Origami;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadLevel4()
    {
        StaticDate.CurrentLevel = 4;
        StaticDate.CountMoves = 40;
        StaticDate.CountCollect = 4;
        StaticDate.TypeBlock = TypeBlock.StarDark;
        SceneManager.LoadScene("GameScene");
    }
    public void LoadLevel5()
    {
        StaticDate.CurrentLevel = 5;
        StaticDate.CountMoves = 50;
        StaticDate.CountCollect = 5;
        StaticDate.TypeBlock = TypeBlock.StarLight;
        SceneManager.LoadScene("GameScene");
    }
    public void LoadLevel6()
    {
        StaticDate.CurrentLevel = 6;
        StaticDate.CountMoves = 60;
        StaticDate.CountCollect = 6;
        StaticDate.TypeBlock = TypeBlock.Racket;
        SceneManager.LoadScene("GameScene");
    }
    public void LoadLevel7()
    {
        StaticDate.CurrentLevel = 7;
        StaticDate.CountMoves = 70;
        StaticDate.CountCollect = 7;
        StaticDate.TypeBlock = TypeBlock.Bucket;
        SceneManager.LoadScene("GameScene");
    }
    public void LoadLevel8()
    {
        StaticDate.CurrentLevel = 8;
        StaticDate.CountMoves = 80;
        StaticDate.CountCollect = 20;
        StaticDate.TypeBlock = TypeBlock.StarLight;
        SceneManager.LoadScene("GameScene");
    }
}
