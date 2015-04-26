// ==================================================================================
// <file="PCG_Tiles.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCG_SurfaceElements : MonoBehaviour
{
    PCG_Rand rand = new PCG_Rand();

    public GameObject _tile;
    public Sprite _tileA, _tileB, _tileC,
    _tileD, _tileE, _tileF, _tileG,
    _tileH, _tileI, _tileJ, _tileK,
    _tileL, _tileM, _tileN, _tileO,
    _tileP, _tileQ, _tileR, _tileS,
    _tileT;

    public Sprite _chasmA, _chasmB, _chasmC;

    public Sprite _rock1, _rock2, _rock3,
    _rock4, _rock5, _rock6, _rock7, _rock8,
    _rock9, _rock10, _rock11, _rock12,
    _rock13, _rock14, _rock15, _rock16,
    _rock17, _rock18, _rock19, _rock20,
    _rock21, _rock22, _rock23, _rock24,
    _rock25, _rock26, _rock27, _rock28,
    _rock29, _rock30, _rock31, _rock32,
    _rock33, _rock34;

    public Sprite _smallRocks1, _smallRocks2, _smallRocks3,
    _smallRocks4, _smallRocks5, _smallRocks6, _smallRocks7,
    _smallRocks8, _smallRocks9, _smallRocks10, _smallRocks11,
    _smallRocks12, _smallRocks13, _smallRocks14, _smallRocks15,
    _smallRocks16;

    float[] _x = new float[11071];
    float[] _y = new float[11071];

    [HideInInspector]
    public bool triggerEntered = false;
    [HideInInspector]
    public bool addRemoveTiles = false;
    [HideInInspector]
    public bool initialFunctionCall = false;
    [HideInInspector]
    public bool moveTrigger = false;

    int vertiLmtINC = 0;
    int vertiLmtINC_again = 0;
    int vertiLmt = 90;
    float _Yincrement = 0;
    float _Xincrement = 1;

    GameObject[] tiles = new GameObject[11071];
    float distBWplayerTiles;

    List<int> smallRocks = new List<int>();
    bool deleteSmallRocksColliders = false;

    float areaSpan = 10.0F;

    #region SpritesGeneration

    public void RndNosGeneration()
    {
        for (int i = 0; i < 11071; i++)
        {
            _y[i] = 4.12F + _Yincrement;
            _Yincrement = _Yincrement + 4.12F;

            if (i == vertiLmt || i == vertiLmt + vertiLmtINC)
            {
                vertiLmtINC = vertiLmtINC + vertiLmt;
                _Yincrement = 0;
            }
        }

        for (int j = 0; j < 11071; j++)
        {
            _x[j] = 4.12F + _Xincrement;

            if (j == vertiLmt || j == vertiLmt + vertiLmtINC_again)
            {
                vertiLmtINC_again = vertiLmtINC_again + vertiLmt;
                _Xincrement = _Xincrement + 4.12F;
            }
        }
    }

    #endregion

    #region OffsetsGeneration

    int RandomNosRange_DeterminingOffset(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.12F) && (rand.seedRndNos_spawning[pos] >= 0.0F)) return 0;        if ((rand.seedRndNos_spawning[pos] <= 0.24f) && (rand.seedRndNos_spawning[pos] >= 0.12f)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 0.36f) && (rand.seedRndNos_spawning[pos] >= 0.24f)) return 2;        if ((rand.seedRndNos_spawning[pos] <= 0.48f) && (rand.seedRndNos_spawning[pos] >= 0.36f)) return 3;
        if ((rand.seedRndNos_spawning[pos] <= 0.60f) && (rand.seedRndNos_spawning[pos] >= 0.48f)) return 4;        if ((rand.seedRndNos_spawning[pos] <= 0.72f) && (rand.seedRndNos_spawning[pos] >= 0.60f)) return 5;
        if ((rand.seedRndNos_spawning[pos] <= 0.84f) && (rand.seedRndNos_spawning[pos] >= 0.72f)) return 6;        if ((rand.seedRndNos_spawning[pos] <= 1.00f) && (rand.seedRndNos_spawning[pos] >= 0.84f)) return 7;
        else return -1;
    }

    void AddOffset(int pos)
    {
        switch (RandomNosRange_DeterminingOffset(pos))
        {
            case -1: Debug.LogError("RndNosOutOfBounds"); break;
            case 0: tiles[pos].transform.position += new Vector3(.45F, 0.0F, 0.0F); break;            case 1: tiles[pos].transform.position += new Vector3(-.45F, 0.0F, 0.0F); break;
            case 2: tiles[pos].transform.position += new Vector3(0.0F, .45F, 0.0F); break;            case 3: tiles[pos].transform.position += new Vector3(0.0F, -.45F, 0.0F); break;
            case 4: tiles[pos].transform.position += new Vector3(.45F, .45F, 0.0F); break;            case 5: tiles[pos].transform.position += new Vector3(-.45F, -.45F, 0.0F); break;
            case 6: tiles[pos].transform.position += new Vector3(.45F, -.45F, 0.0F); break;            case 7: tiles[pos].transform.position += new Vector3(-.45F, .45F, 0.0F); break;
            default: break;
        }
    }

    #endregion

    int RandomNosRange_ChangingSpritesOverall(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.50F) && (rand.seedRndNos_spawning[pos] >= 0.00F)) return 0;
        if ((rand.seedRndNos_spawning[pos] <= 0.75F) && (rand.seedRndNos_spawning[pos] >= 0.50F)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 1.00F) && (rand.seedRndNos_spawning[pos] >= 0.75F)) return 2;
        else return -1;
    }

    void ChangeSprites(int pos)
    {
        switch (RandomNosRange_ChangingSpritesOverall(pos))
        {
            case -1: Debug.LogError("RndNosOutOfBounds"); break;
            case 0: ChangeSpritesToCraters(pos); break;
            case 1: ChangeSpritesToChasms(pos); break;
            case 2: ChangeSpritesToRocks(pos); break;
            default: break;
        }
    }

    int RandomNosRange_ChangingSpritesToCraters(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.05F) && (rand.seedRndNos_spawning[pos] >= 0.00F)) return 0;        if ((rand.seedRndNos_spawning[pos] <= 0.10F) && (rand.seedRndNos_spawning[pos] >= 0.05F)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 0.15F) && (rand.seedRndNos_spawning[pos] >= 0.10F)) return 2;        if ((rand.seedRndNos_spawning[pos] <= 0.20F) && (rand.seedRndNos_spawning[pos] >= 0.15F)) return 3;
        if ((rand.seedRndNos_spawning[pos] <= 0.25F) && (rand.seedRndNos_spawning[pos] >= 0.20F)) return 4;        if ((rand.seedRndNos_spawning[pos] <= 0.30F) && (rand.seedRndNos_spawning[pos] >= 0.25F)) return 5;
        if ((rand.seedRndNos_spawning[pos] <= 0.35F) && (rand.seedRndNos_spawning[pos] >= 0.30F)) return 6;        if ((rand.seedRndNos_spawning[pos] <= 0.40F) && (rand.seedRndNos_spawning[pos] >= 0.35F)) return 7;
        if ((rand.seedRndNos_spawning[pos] <= 0.45F) && (rand.seedRndNos_spawning[pos] >= 0.40F)) return 8;        if ((rand.seedRndNos_spawning[pos] <= 0.50F) && (rand.seedRndNos_spawning[pos] >= 0.45F)) return 9;
        if ((rand.seedRndNos_spawning[pos] <= 0.55F) && (rand.seedRndNos_spawning[pos] >= 0.50F)) return 10;        if ((rand.seedRndNos_spawning[pos] <= 0.60F) && (rand.seedRndNos_spawning[pos] >= 0.55F)) return 11;
        if ((rand.seedRndNos_spawning[pos] <= 0.65F) && (rand.seedRndNos_spawning[pos] >= 0.60F)) return 12;        if ((rand.seedRndNos_spawning[pos] <= 0.70F) && (rand.seedRndNos_spawning[pos] >= 0.65F)) return 13;
        if ((rand.seedRndNos_spawning[pos] <= 0.75F) && (rand.seedRndNos_spawning[pos] >= 0.70F)) return 14;        if ((rand.seedRndNos_spawning[pos] <= 0.80F) && (rand.seedRndNos_spawning[pos] >= 0.75F)) return 15;
        if ((rand.seedRndNos_spawning[pos] <= 0.85F) && (rand.seedRndNos_spawning[pos] >= 0.80F)) return 16;        if ((rand.seedRndNos_spawning[pos] <= 0.90F) && (rand.seedRndNos_spawning[pos] >= 0.85F)) return 17;
        if ((rand.seedRndNos_spawning[pos] <= 0.95F) && (rand.seedRndNos_spawning[pos] >= 0.90F)) return 18;        if ((rand.seedRndNos_spawning[pos] <= 1.00F) && (rand.seedRndNos_spawning[pos] >= 0.95F)) return 19;
        else return -1;
    }

    void ChangeSpritesToCraters(int pos)
    {
        switch (RandomNosRange_ChangingSpritesToCraters(pos))
        {
            case 0: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileA; ChangeColliderDimensions(pos, 3); break;            case 1: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileB; ChangeColliderDimensions(pos, 4); break;
            case 2: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileC; ChangeColliderDimensions(pos, 5); break;            case 3: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileD; ChangeColliderDimensions(pos, 6); break;
            case 4: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileE; ChangeColliderDimensions(pos, 7); break;            case 5: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileF; ChangeColliderDimensions(pos, 8); break;
            case 6: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileG; ChangeColliderDimensions(pos, 9); break;            case 7: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileH; ChangeColliderDimensions(pos, 10); break;
            case 8: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileI; ChangeColliderDimensions(pos, 11); break;            case 9: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileJ; ChangeColliderDimensions(pos, 12); break;
            case 10: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileK; ChangeColliderDimensions(pos, 13); break;            case 11: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileL; ChangeColliderDimensions(pos, 14); break;
            case 12: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileM; ChangeColliderDimensions(pos, 15); break;            case 13: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileN; ChangeColliderDimensions(pos, 16); break;
            case 14: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileO; ChangeColliderDimensions(pos, 17); break;            case 15: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileP; ChangeColliderDimensions(pos, 18); break;
            case 16: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileQ; ChangeColliderDimensions(pos, 19); break;            case 17: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileR; ChangeColliderDimensions(pos, 20); break;
            case 18: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileS; ChangeColliderDimensions(pos, 21); break;            case 19: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _tileT; ChangeColliderDimensions(pos, 22); break;
            default: break;
        }
    }

    int RandomNosRange_ChangingSpritesToChasms(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.33F) && (rand.seedRndNos_spawning[pos] >= 0.00F)) return 0;
        if ((rand.seedRndNos_spawning[pos] <= 0.66F) && (rand.seedRndNos_spawning[pos] >= 0.33F)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 1.00F) && (rand.seedRndNos_spawning[pos] >= 0.66F)) return 2;
        else return -1;
    }

    void ChangeSpritesToChasms(int pos)
    {
        switch (RandomNosRange_ChangingSpritesToChasms(pos))
        {
            case 0: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _chasmA; ChangeColliderDimensions(pos, 0); break;
            case 1: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _chasmB; ChangeColliderDimensions(pos, 1); break;
            case 2: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _chasmC; ChangeColliderDimensions(pos, 2); break;
            default: break;
        }
    }

    int RandomNosRange_ChangingSpritesToRocks(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.029F) && (rand.seedRndNos_spawning[pos] >= 0.000F)) return 0;        if ((rand.seedRndNos_spawning[pos] <= 0.058F) && (rand.seedRndNos_spawning[pos] >= 0.029F)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 0.087F) && (rand.seedRndNos_spawning[pos] >= 0.058F)) return 2;        if ((rand.seedRndNos_spawning[pos] <= 0.116F) && (rand.seedRndNos_spawning[pos] >= 0.087F)) return 3;
        if ((rand.seedRndNos_spawning[pos] <= 0.145F) && (rand.seedRndNos_spawning[pos] >= 0.116F)) return 4;        if ((rand.seedRndNos_spawning[pos] <= 0.174F) && (rand.seedRndNos_spawning[pos] >= 0.145F)) return 5;
        if ((rand.seedRndNos_spawning[pos] <= 0.203F) && (rand.seedRndNos_spawning[pos] >= 0.174F)) return 6;        if ((rand.seedRndNos_spawning[pos] <= 0.232F) && (rand.seedRndNos_spawning[pos] >= 0.203F)) return 7;
        if ((rand.seedRndNos_spawning[pos] <= 0.261F) && (rand.seedRndNos_spawning[pos] >= 0.232F)) return 8;        if ((rand.seedRndNos_spawning[pos] <= 0.290F) && (rand.seedRndNos_spawning[pos] >= 0.261F)) return 9;
        if ((rand.seedRndNos_spawning[pos] <= 0.319F) && (rand.seedRndNos_spawning[pos] >= 0.290F)) return 10;        if ((rand.seedRndNos_spawning[pos] <= 0.348F) && (rand.seedRndNos_spawning[pos] >= 0.319F)) return 11;
        if ((rand.seedRndNos_spawning[pos] <= 0.377F) && (rand.seedRndNos_spawning[pos] >= 0.348F)) return 12;        if ((rand.seedRndNos_spawning[pos] <= 0.406F) && (rand.seedRndNos_spawning[pos] >= 0.377F)) return 13;
        if ((rand.seedRndNos_spawning[pos] <= 0.435F) && (rand.seedRndNos_spawning[pos] >= 0.406F)) return 14;        if ((rand.seedRndNos_spawning[pos] <= 0.464F) && (rand.seedRndNos_spawning[pos] >= 0.435F)) return 15;
        if ((rand.seedRndNos_spawning[pos] <= 0.493F) && (rand.seedRndNos_spawning[pos] >= 0.464F)) return 16;        if ((rand.seedRndNos_spawning[pos] <= 0.522F) && (rand.seedRndNos_spawning[pos] >= 0.493F)) return 17;
        if ((rand.seedRndNos_spawning[pos] <= 0.551F) && (rand.seedRndNos_spawning[pos] >= 0.522F)) return 18;        if ((rand.seedRndNos_spawning[pos] <= 0.580F) && (rand.seedRndNos_spawning[pos] >= 0.551F)) return 19;
        if ((rand.seedRndNos_spawning[pos] <= 0.609F) && (rand.seedRndNos_spawning[pos] >= 0.580F)) return 20;        if ((rand.seedRndNos_spawning[pos] <= 0.638F) && (rand.seedRndNos_spawning[pos] >= 0.609F)) return 21;
        if ((rand.seedRndNos_spawning[pos] <= 0.667F) && (rand.seedRndNos_spawning[pos] >= 0.638F)) return 22;        if ((rand.seedRndNos_spawning[pos] <= 0.696F) && (rand.seedRndNos_spawning[pos] >= 0.667F)) return 23;
        if ((rand.seedRndNos_spawning[pos] <= 0.725F) && (rand.seedRndNos_spawning[pos] >= 0.696F)) return 24;        if ((rand.seedRndNos_spawning[pos] <= 0.754F) && (rand.seedRndNos_spawning[pos] >= 0.725F)) return 25;
        if ((rand.seedRndNos_spawning[pos] <= 0.783F) && (rand.seedRndNos_spawning[pos] >= 0.754F)) return 26;        if ((rand.seedRndNos_spawning[pos] <= 0.812F) && (rand.seedRndNos_spawning[pos] >= 0.783F)) return 27;
        if ((rand.seedRndNos_spawning[pos] <= 0.841F) && (rand.seedRndNos_spawning[pos] >= 0.812F)) return 28;        if ((rand.seedRndNos_spawning[pos] <= 0.870F) && (rand.seedRndNos_spawning[pos] >= 0.841F)) return 29;
        if ((rand.seedRndNos_spawning[pos] <= 0.899F) && (rand.seedRndNos_spawning[pos] >= 0.870F)) return 30;        if ((rand.seedRndNos_spawning[pos] <= 0.928F) && (rand.seedRndNos_spawning[pos] >= 0.899F)) return 31;
        if ((rand.seedRndNos_spawning[pos] <= 0.957F) && (rand.seedRndNos_spawning[pos] >= 0.928F)) return 32;        if ((rand.seedRndNos_spawning[pos] <= 1.000F) && (rand.seedRndNos_spawning[pos] >= 0.957F)) return 33;
        else return -1;
    }

    void ChangeSpritesToRocks(int pos)
    {
        switch (RandomNosRange_ChangingSpritesToRocks(pos))
        {
            case 0: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock1; ChangeColliderDimensions(pos, 23); break;            case 1: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock2; ChangeColliderDimensions(pos, 24); break;
            case 2: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock3; ChangeColliderDimensions(pos, 25); break;            case 3: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock4; ChangeColliderDimensions(pos, 26); break;
            case 4: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock5; ChangeColliderDimensions(pos, 27); break;            case 5: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock6; ChangeColliderDimensions(pos, 28); break;
            case 6: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock7; ChangeColliderDimensions(pos, 29); break;            case 7: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock8; ChangeColliderDimensions(pos, 30); break;
            case 8: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock9; ChangeColliderDimensions(pos, 31); break;            case 9: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock10; ChangeColliderDimensions(pos, 32); break;
            case 10: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock11; ChangeColliderDimensions(pos, 33); break;            case 11: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock12; ChangeColliderDimensions(pos, 34); break;
            case 12: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock13; ChangeColliderDimensions(pos, 35); break;            case 13: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock14; ChangeColliderDimensions(pos, 36); break;
            case 14: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock15; ChangeColliderDimensions(pos, 37); break;            case 15: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock16; ChangeColliderDimensions(pos, 38); break;
            case 16: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock17; ChangeColliderDimensions(pos, 39); break;            case 17: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock18; ChangeColliderDimensions(pos, 40); break;
            case 18: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock19; ChangeColliderDimensions(pos, 41); break;            case 19: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock20; ChangeColliderDimensions(pos, 42); break;
            case 20: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock21; ChangeColliderDimensions(pos, 43); break;            case 21: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock22; ChangeColliderDimensions(pos, 44); break;
            case 22: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock23; ChangeColliderDimensions(pos, 45); break;            case 23: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock24; ChangeColliderDimensions(pos, 46); break;
            case 24: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock25; ChangeColliderDimensions(pos, 47); break;            case 25: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock26; ChangeColliderDimensions(pos, 48); break;
            case 26: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock27; ChangeColliderDimensions(pos, 49); break;            case 27: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock28; ChangeColliderDimensions(pos, 50); break;
            case 28: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock29; ChangeColliderDimensions(pos, 51); break;            case 29: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock30; ChangeColliderDimensions(pos, 52); break;
            case 30: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock31; ChangeColliderDimensions(pos, 53); break;            case 31: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock32; ChangeColliderDimensions(pos, 54); break;
            case 32: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock33; ChangeColliderDimensions(pos, 55); break;            case 33: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rock34; ChangeColliderDimensions(pos, 56); break;
            default: break;
        }
    }

    int RandomNosRange_ChangingSpritesToSmallRocks(int pos)
    {
        if ((rand.seedRndNos_deleting[pos] <= 0.066F) && (rand.seedRndNos_deleting[pos] >= 0.000F)) return 0;        if ((rand.seedRndNos_deleting[pos] <= 0.132F) && (rand.seedRndNos_deleting[pos] >= 0.066F)) return 1;
        if ((rand.seedRndNos_deleting[pos] <= 0.198F) && (rand.seedRndNos_deleting[pos] >= 0.132F)) return 2;        if ((rand.seedRndNos_deleting[pos] <= 0.264F) && (rand.seedRndNos_deleting[pos] >= 0.198F)) return 3;
        if ((rand.seedRndNos_deleting[pos] <= 0.330F) && (rand.seedRndNos_deleting[pos] >= 0.264F)) return 4;        if ((rand.seedRndNos_deleting[pos] <= 0.396F) && (rand.seedRndNos_deleting[pos] >= 0.330F)) return 5;
        if ((rand.seedRndNos_deleting[pos] <= 0.462F) && (rand.seedRndNos_deleting[pos] >= 0.396F)) return 6;        if ((rand.seedRndNos_deleting[pos] <= 0.528F) && (rand.seedRndNos_deleting[pos] >= 0.462F)) return 7;
        if ((rand.seedRndNos_deleting[pos] <= 0.594F) && (rand.seedRndNos_deleting[pos] >= 0.528F)) return 8;        if ((rand.seedRndNos_deleting[pos] <= 0.660F) && (rand.seedRndNos_deleting[pos] >= 0.594F)) return 9;
        if ((rand.seedRndNos_deleting[pos] <= 0.726F) && (rand.seedRndNos_deleting[pos] >= 0.660F)) return 10;        if ((rand.seedRndNos_deleting[pos] <= 0.792F) && (rand.seedRndNos_deleting[pos] >= 0.726F)) return 11;
        if ((rand.seedRndNos_deleting[pos] <= 0.858F) && (rand.seedRndNos_deleting[pos] >= 0.792F)) return 12;        if ((rand.seedRndNos_deleting[pos] <= 0.924F) && (rand.seedRndNos_deleting[pos] >= 0.858F)) return 13;
        if ((rand.seedRndNos_deleting[pos] <= 1.000F) && (rand.seedRndNos_deleting[pos] >= 0.924F)) return 14;        else return -1;
    }

    void ChangeSpritesToSmallRocks(int pos)
    {
        switch (RandomNosRange_ChangingSpritesToSmallRocks(pos))
        {
            case 0: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks1; break;            case 1: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks2; break;
            case 2: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks3; break;            case 3: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks4; break;
            case 4: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks5; break;            case 5: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks6; break;
            case 6: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks7; break;            case 7: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks8; break;
            case 8: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks9; break;            case 9: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks10; break;
            case 10: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks11; break;            case 11: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks12; break;
            case 12: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks13; break;            case 13: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks14; break;
            case 14: tiles[pos].GetComponentInChildren<SpriteRenderer>().sprite = _smallRocks15; break;            default: break;
        }
    }

    void AddRemoveTiles()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        if (triggerEntered == true || initialFunctionCall == false)
        {
            initialFunctionCall = true;
            if (!addRemoveTiles)
            {
                addRemoveTiles = true;
                triggerEntered = false;
                moveTrigger = false;

                GameObject[] gameObjectTile = GameObject.FindGameObjectsWithTag("CratersOnScreen");
                List<GameObject> gameObjectTileList = gameObjectTile.ToList();
                List<GameObject> finalList = gameObjectTileList.Distinct().ToList();

                GameObject[] gameObjectTile2 = GameObject.FindGameObjectsWithTag("SmallRocksOnScreen");
                List<GameObject> gameObjectTileList2 = gameObjectTile2.ToList();
                List<GameObject> finalList2 = gameObjectTileList2.Distinct().ToList();

                for (int i = 0; i < 11071; i++)
                {
                    distBWplayerTiles = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                    if (distBWplayerTiles < areaSpan)
                    {
                        tiles[i] = Instantiate(_tile, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                        tiles[i].tag = "CratersOnScreen";
                        tiles[i].name = "Craters " + i;

                        if (tiles[i].name == "Craters 90") { tiles[i].GetComponentInChildren<SpriteRenderer>().enabled = false; }
                        ChangeSprites(i);
                        RND_DeleteExtraSpawns(i);
                        AddOffset(i);

                        foreach (GameObject item in finalList)
                        {
                            if (item.name == tiles[i].name) { Destroy(tiles[i]); }
                        }

                        foreach (GameObject item in finalList2)
                        {
                            if (item.name == tiles[i].name) { Destroy(tiles[i]); }
                        }

                        foreach (Transform child in tiles[i].transform)
                        {
                            Destroy(child.GetComponent<PCG_OptimizePolygonCollider2D>());
                            child.gameObject.AddComponent<PCG_OptimizePolygonCollider2D>();
                        }
                    }

                    if (i == 11070) { deleteSmallRocksColliders = true; }
                }

                GameObject[] allTexturesMyFriend = GameObject.FindGameObjectsWithTag("CratersOnScreen");
                foreach (GameObject texture in allTexturesMyFriend)
                {
                    var distance2 = Vector2.Distance(texture.transform.position, mainPlayerPos);
                    if (distance2 > areaSpan) { GameObject.Destroy(texture); }
                }

                GameObject[] allTexturesMyFriend2 = GameObject.FindGameObjectsWithTag("SmallRocksOnScreen");
                foreach (GameObject texture in allTexturesMyFriend2)
                {
                    var distance2 = Vector2.Distance(texture.transform.position, mainPlayerPos);
                    if (distance2 > areaSpan) { GameObject.Destroy(texture); }
                }
            }
        }
    }
    int RandomNosRangeToExactValues(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.05F) && (rand.seedRndNos_spawning[pos] >= 0.0F)) return 0;        if ((rand.seedRndNos_spawning[pos] <= 0.10f) && (rand.seedRndNos_spawning[pos] > 0.05f)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 0.15f) && (rand.seedRndNos_spawning[pos] > 0.10f)) return 2;        if ((rand.seedRndNos_spawning[pos] <= 0.20f) && (rand.seedRndNos_spawning[pos] > 0.15f)) return 3;
        if ((rand.seedRndNos_spawning[pos] <= 0.25f) && (rand.seedRndNos_spawning[pos] > 0.20f)) return 4;        if ((rand.seedRndNos_spawning[pos] <= 0.30f) && (rand.seedRndNos_spawning[pos] > 0.25f)) return 5;
        if ((rand.seedRndNos_spawning[pos] <= 0.35f) && (rand.seedRndNos_spawning[pos] > 0.30f)) return 6;        if ((rand.seedRndNos_spawning[pos] <= 0.40f) && (rand.seedRndNos_spawning[pos] > 0.35f)) return 7;
        if ((rand.seedRndNos_spawning[pos] <= 0.45f) && (rand.seedRndNos_spawning[pos] > 0.40f)) return 8;        if ((rand.seedRndNos_spawning[pos] <= 0.50f) && (rand.seedRndNos_spawning[pos] > 0.45f)) return 9;
        if ((rand.seedRndNos_spawning[pos] <= 0.55f) && (rand.seedRndNos_spawning[pos] > 0.50f)) return 10;       if ((rand.seedRndNos_spawning[pos] <= 0.60f) && (rand.seedRndNos_spawning[pos] > 0.55f)) return 11;
        if ((rand.seedRndNos_spawning[pos] <= 0.65f) && (rand.seedRndNos_spawning[pos] > 0.60f)) return 12;       if ((rand.seedRndNos_spawning[pos] <= 0.70f) && (rand.seedRndNos_spawning[pos] > 0.65f)) return 13;
        if ((rand.seedRndNos_spawning[pos] <= 0.75f) && (rand.seedRndNos_spawning[pos] > 0.70f)) return 14;       if ((rand.seedRndNos_spawning[pos] <= 0.80f) && (rand.seedRndNos_spawning[pos] > 0.75f)) return 15;
        if ((rand.seedRndNos_spawning[pos] <= 0.85f) && (rand.seedRndNos_spawning[pos] > 0.80f)) return 16;       if ((rand.seedRndNos_spawning[pos] <= 0.90f) && (rand.seedRndNos_spawning[pos] > 0.85f)) return 17;
        if ((rand.seedRndNos_spawning[pos] <= 0.95f) && (rand.seedRndNos_spawning[pos] > 0.90f)) return 18;       if ((rand.seedRndNos_spawning[pos] <= 1.00f) && (rand.seedRndNos_spawning[pos] > 0.95f)) return 19;
        else return -1;
    }

    int RandonNosRangeToExactValuesForDeletingExtraSpawns(int pos)
    {
        if ((rand.seedRndNos_deleting[pos] <= 0.60F) && (rand.seedRndNos_deleting[pos] >= 0.00F)) return 0;
        if ((rand.seedRndNos_deleting[pos] <= 0.70f) && (rand.seedRndNos_deleting[pos] >= 0.60f)) return 1;
        if ((rand.seedRndNos_deleting[pos] <= 1.00f) && (rand.seedRndNos_deleting[pos] >= 0.70f)) return 2;
        else return -1;
    }

    void RND_DeleteExtraSpawns(int pos)
    {
        switch (RandonNosRangeToExactValuesForDeletingExtraSpawns(pos))
        {
            case -1: Debug.LogError("RndNosOutOfBounds"); break;
            case 0: Destroy(tiles[pos]); break;
            case 1: ChangeSpritesToSmallRocks(pos);
                    smallRocks.Add(pos);
                    tiles[pos].tag = "SmallRocksOnScreen"; break;
            case 2: break;
            default: return;
        }
    }

    void DeletingColliders_SmallRocks()
    {
        if (deleteSmallRocksColliders)
        {
            deleteSmallRocksColliders = false;
            foreach (int _pos in smallRocks)
            {
                GameObject[] surfaceElements = GameObject.FindGameObjectsWithTag("SmallRocksOnScreen");
                foreach (GameObject element in surfaceElements)
                {
                    if (element.name == "Craters " + _pos)
                    {
                        foreach (Transform child in element.transform)
                        {
                            Destroy(child.GetComponent<PolygonCollider2D>());
                            Destroy(child.GetComponent<BoxCollider2D>());
                            Destroy(child.GetComponent<CircleCollider2D>());
                        }
                    }
                }
            }
        }
    }

    void Start()
    {
        rand = FindObjectOfType(typeof(PCG_Rand)) as PCG_Rand;
        RndNosGeneration();
    }

    void FixedUpdate()
    {
        DeletingColliders_SmallRocks();
    }

    void Update()
    {
        AddRemoveTiles();
    }

    void ChangeColliderDimensions(int pos, int index)
    {
        switch (index)
        {
            case 0: ChangeColliderDimensions_Chasms(pos, 0); break;         case 1: ChangeColliderDimensions_Chasms(pos, 1); break;         case 2: ChangeColliderDimensions_Chasms(pos, 2); break;         case 3: ChangeColliderDimensions_Craters(pos, 0); break;
            case 4: ChangeColliderDimensions_Craters(pos, 1); break;        case 5: ChangeColliderDimensions_Craters(pos, 2); break;        case 6: ChangeColliderDimensions_Craters(pos, 3); break;        case 7: ChangeColliderDimensions_Craters(pos, 4); break;
            case 8: ChangeColliderDimensions_Craters(pos, 5); break;        case 9: ChangeColliderDimensions_Craters(pos, 6); break;        case 10: ChangeColliderDimensions_Craters(pos, 7); break;       case 11: ChangeColliderDimensions_Craters(pos, 8); break;
            case 12: ChangeColliderDimensions_Craters(pos, 9); break;       case 13: ChangeColliderDimensions_Craters(pos, 10); break;      case 14: ChangeColliderDimensions_Craters(pos, 11); break;      case 15: ChangeColliderDimensions_Craters(pos, 12); break;
            case 16: ChangeColliderDimensions_Craters(pos, 13); break;      case 17: ChangeColliderDimensions_Craters(pos, 14); break;      case 18: ChangeColliderDimensions_Craters(pos, 15); break;      case 19: ChangeColliderDimensions_Craters(pos, 16); break;
            case 20: ChangeColliderDimensions_Craters(pos, 17); break;      case 21: ChangeColliderDimensions_Craters(pos, 18); break;      case 22: ChangeColliderDimensions_Craters(pos, 19); break;      case 23: ChangeColliderDimensions_Rocks(pos); break;
            case 24: ChangeColliderDimensions_Rocks(pos); break;            case 25: ChangeColliderDimensions_Rocks(pos); break;            case 26: ChangeColliderDimensions_Rocks(pos); break;            case 27: ChangeColliderDimensions_Rocks(pos); break;
            case 28: ChangeColliderDimensions_Rocks(pos); break;            case 29: ChangeColliderDimensions_Rocks(pos); break;            case 30: ChangeColliderDimensions_Rocks(pos); break;            case 31: ChangeColliderDimensions_Rocks(pos); break;
            case 32: ChangeColliderDimensions_Rocks(pos); break;            case 33: ChangeColliderDimensions_Rocks(pos); break;            case 34: ChangeColliderDimensions_Rocks(pos); break;            case 35: ChangeColliderDimensions_Rocks(pos); break;
            case 36: ChangeColliderDimensions_Rocks(pos); break;            case 37: ChangeColliderDimensions_Rocks(pos); break;            case 38: ChangeColliderDimensions_Rocks(pos); break;            case 39: ChangeColliderDimensions_Rocks(pos); break;
            case 40: ChangeColliderDimensions_Rocks(pos); break;            case 41: ChangeColliderDimensions_Rocks(pos); break;            case 42: ChangeColliderDimensions_Rocks(pos); break;            case 43: ChangeColliderDimensions_Rocks(pos); break;
            case 44: ChangeColliderDimensions_Rocks(pos); break;            case 45: ChangeColliderDimensions_Rocks(pos); break;            case 46: ChangeColliderDimensions_Rocks(pos); break;            case 47: ChangeColliderDimensions_Rocks(pos); break;
            case 48: ChangeColliderDimensions_Rocks(pos); break;            case 49: ChangeColliderDimensions_Rocks(pos); break;            case 50: ChangeColliderDimensions_Rocks(pos); break;            case 51: ChangeColliderDimensions_Rocks(pos); break;
            case 52: ChangeColliderDimensions_Rocks(pos); break;            case 53: ChangeColliderDimensions_Rocks(pos); break;            case 54: ChangeColliderDimensions_Rocks(pos); break;            case 55: ChangeColliderDimensions_Rocks(pos); break;
            case 56: ChangeColliderDimensions_Rocks(pos); break;            default: break;
        }
    }

    void ChangeColliderDimensions_Chasms(int pos, int index)
    {
        switch (index)
        {
            case 0: foreach (Transform child in tiles[pos].transform)
                {
                    Destroy(child.GetComponent<PolygonCollider2D>());
                    child.gameObject.AddComponent<PolygonCollider2D>();
                    child.gameObject.GetComponent<BoxCollider2D>().center = new Vector2(0.00F, -0.13F);
                    child.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(2.97F, 3.78F);
                } break;
            case 1: foreach (Transform child in tiles[pos].transform)
                {
                    Destroy(child.GetComponent<PolygonCollider2D>());
                    child.gameObject.AddComponent<PolygonCollider2D>();
                    child.gameObject.GetComponent<BoxCollider2D>().center = new Vector2(0.00F, -0.01F);
                    child.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(3.89F, 4.02F);
                } break;
            case 2: foreach (Transform child in tiles[pos].transform)
                {
                    Destroy(child.GetComponent<PolygonCollider2D>());
                    child.gameObject.AddComponent<PolygonCollider2D>();
                    child.gameObject.GetComponent<BoxCollider2D>().center = new Vector2(0.00F, -0.01F);
                    child.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(3.89F, 4.02F);
                } break;
            default: break;
        }
    }

    void ChangeColliderDimensions_Craters(int pos, int index)
    {
        foreach (Transform child in tiles[pos].transform)
        {
            Destroy(child.GetComponent<PolygonCollider2D>());
            Destroy(child.GetComponent<BoxCollider2D>());
            child.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }

        switch (index)
        {
            case 0: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.03F, 0.05F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.65F; } break;
            case 1: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.03F, 0.10F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.64F; } break;
            case 2: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.04F, -0.23F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.49F; } break;
            case 3: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.08F, 0.09F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.42F; } break;
            case 4: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.00F, -0.13F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.34F; } break;
            case 5: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.00F, -0.13F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.34F; } break;
            case 6: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.17F, -0.07F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.34F; } break;
            case 7: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.01F, -0.07F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.16F; } break;
            case 8: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.07F, -0.07F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.34F; } break;
            case 9: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.00F, 0.01F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.54F; } break;
            case 10: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.08F, -0.06F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.20F; } break;
            case 11: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.08F, -0.06F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.18F; } break;
            case 12: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.08F, -0.06F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.39F; } break;
            case 13: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.08F, -0.14F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.57F; } break;
            case 14: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.08F, -0.14F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.57F; } break;
            case 15: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.11F, -0.14F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.58F; } break;
            case 16: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.11F, -0.14F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.58F; } break;
            case 17: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(-0.11F, -0.14F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.58F; } break;
            case 18: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.0F, -0.01F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.59F; } break;
            case 19: foreach (Transform child in tiles[pos].transform) { child.gameObject.GetComponent<CircleCollider2D>().center = new Vector2(0.0F, -0.01F); child.gameObject.GetComponent<CircleCollider2D>().radius = 1.59F; } break;
            default: break;
        }
    }

    void ChangeColliderDimensions_Rocks(int pos)
    {
        foreach (Transform child in tiles[pos].transform)
        {
            Destroy(child.GetComponent<PolygonCollider2D>());
            Destroy(child.GetComponent<CircleCollider2D>());
            child.gameObject.AddComponent<PolygonCollider2D>();
            child.gameObject.GetComponent<BoxCollider2D>().center = new Vector2(0.06F, 0.00F);
            child.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(3.63F, 3.05F);
        }
    }
}
