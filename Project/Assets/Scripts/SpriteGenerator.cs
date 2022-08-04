using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGenerator : MonoBehaviour
{
    #region Statics
    static SpriteGenerator instance;
    public static SpriteGenerator I
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance) { return; }
            else
            {
                instance = value;
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"Already a {name} in scene. Deleting this one!");
            Destroy(gameObject);
            return;
        }
    }

    #endregion

    public Sprite ConvertToSprite(Texture2D texture, int pixelsPerUnit)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), pixelsPerUnit);
        return sprite;
    }

    Dictionary<CharacterData, Texture2D> getT;

    public Texture2D GetHuman(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];

        Vector2Int startPos = new Vector2Int(width / 2, height / 2);
        Vector2Int walker = startPos;

        bool done = false;

        Vector2Int dir = GetDir(new Vector2Int(-1, 0));
        Vector2Int currentStep = Vector2Int.zero;
        int changes = 0;

        int times = 0;

        while (changes < 5)
        {
            times++;

            if (Random.Range(0, 100) <= times) { ChangeDirection(false); times = 0; }

            Vector2Int step = Vector2Int.zero;

            if (currentStep == dir) { currentStep = Vector2Int.zero; }

            if (dir.x > 0)
            {
                if (dir.x > currentStep.x)
                {
                    currentStep.x++;
                    step.x++;
                }
            }
            else
            {
                if (dir.x < currentStep.x)
                {
                    currentStep.x--;
                    step.x--;
                }
            }

            if (dir.y > 0)
            {
                if (dir.y > currentStep.y)
                {
                    currentStep.y++;
                    step.y++;
                }
            }
            else
            {
                if (dir.y < currentStep.y)
                {
                    currentStep.y--;
                    step.y--;
                }
            }

            if (step.x + walker.x > width - 1 || step.x + walker.x < 0 || step.y + walker.y > height - 1 || step.y + walker.y < 0)
            { ChangeDirection(true); continue; }

            walker += step;

            #region Old
            //// Closer to the left, the higher the number
            //// Closer to the right, the lower the number
            //int xWeight = (width / 2 - walker.x);

            //int yWeight = (height / 2 - walker.y);

            //// If the walker is close to the left go down.
            //if (Random.Range(0, (width / 4)) <= xWeight)
            //{
            //    walker.y--;
            //}

            //// If the walker is close to the right go up.
            //else if (Random.Range((-width / 4), 0) >= xWeight)
            //{
            //    walker.y++;
            //}

            //// If the walker is close to the top go right.
            //if (Random.Range(0, (height / 4)) <= yWeight)
            //{
            //    walker.x++;
            //}

            //// If the walker is close to the bottom go left.
            //else if (Random.Range((-height / 4), 0) >= yWeight)
            //{
            //    walker.x--;
            //}
            #endregion

            if (walker.x >= 0 && walker.x <= width - 1 && walker.y >= 0 && walker.y <= height - 1)
            {
                pixels[GetIndex(width, walker.x, walker.y)] = color;
            }

            done = true;
        }

        Vector2 fwalker = walker;
        bool connected = false;

        while (!connected)
        {
            if (walker == startPos) { connected = true; }

            fwalker = Vector2.MoveTowards(fwalker, startPos, 1 / Vector2.Distance(fwalker, startPos));

            walker = new Vector2Int((int)fwalker.x, (int)fwalker.y);

            if (walker.x >= 0 && walker.x <= width - 1 && walker.y >= 0 && walker.y <= height - 1)
            {
                pixels[GetIndex(width, walker.x, walker.y)] = color;
            }
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        texture.filterMode = FilterMode.Point;

        return Fill(texture);

        Vector2Int GetDir(Vector2Int preference)
        {
            Vector2Int dir = Vector2Int.zero;

            if (preference.y < 0)
            {
                dir.y = Random.Range(-3, 0);
            }
            else if (preference.y > 0)
            {
                dir.y = Random.Range(1, 4);
            }
            else if (Random.Range(0, 2) == 0)
            {
                if (Random.Range(0, 2) == 0)
                {
                    dir.y = Random.Range(-3, 0);
                }
                else
                {
                    dir.y = Random.Range(1, 4);
                }
            }

            if (preference.x < 0)
            {
                dir.x = Random.Range(-3, 0);
            }
            else if (preference.x > 0)
            {
                dir.x = Random.Range(1, 4);
            }
            else if (Random.Range(0, 2) == 0)
            {
                if (Random.Range(0, 2) == 0)
                {
                    dir.x = Random.Range(-3, 0);
                }
                else
                {
                    dir.x = Random.Range(1, 4);
                }
            }

            return dir;
        }

        void ChangeDirection(bool hitWall)
        {
            Vector2Int preference = Vector2Int.zero;

            if (hitWall)
            {

            }

            //    case 4:
            //            preference = new Vector2Int(-1, -1);
            //break;
            //        case 5:
            //            preference = new Vector2Int(1, -1);
            //break;
            //        case 6:
            //            preference = new Vector2Int(1, 1);
            //break;
            //        case 7:
            //            preference = new Vector2Int(-1, 1);
            //break;

            switch (changes)
            {
                case 0:
                    preference = new Vector2Int(-1, 0);
                    break;
                case 1:
                    preference = new Vector2Int(0, -1);
                    break;
                case 2:
                    preference = new Vector2Int(1, 0);
                    break;
                case 3:
                    preference = new Vector2Int(0, 1);
                    break;
                case 4:
                    preference = new Vector2Int(-1, 0);
                    break;
                    //case 5:
                    //    preference = new Vector2Int(1, -1);
                    //    break;
                    //case 6:
                    //    preference = new Vector2Int(1, 1);
                    //    break;
                    //case 7:
                    //    preference = new Vector2Int(-1, 1);
                    //    break;
            }

            currentStep = Vector2Int.zero;
            dir = GetDir(preference);
            changes++;
        }
    }

    int GetIndex(int width, int x, int y)
    {
        return (y * width) + x;
    }

    public Texture2D Fill(Texture2D t)
    {
        for (int x = 0; x < t.width; x++)
            for (int y = 0; y < t.width; y++)
            {
                if (HasNeighbor(t, x, y, false))
                {
                    t.SetPixel(x, y, Color.black);
                }
            }

        return t;
    }

    public Texture2D GenerateOutline(Texture2D t, bool diagonal)
    {
        Texture2D newT = new Texture2D(t.width, t.height);
        newT.SetPixels(t.GetPixels());
        newT.filterMode = t.filterMode;

        for (int x = 0; x < t.width; x++)
        {
            for (int y = 0; y < t.height; y++)
            {
                if (HasNeighbor(t, x, y, diagonal))
                {
                    newT.SetPixel(x, y, Color.black);
                }
            }
        }

        newT.Apply();

        return newT;
    }

    bool HasNeighbor(Texture2D t, int x, int y, bool diagonal)
    {
        if (t.GetPixel(x, y).a == 1) { return false; }

        if (x < t.width - 1 && t.GetPixel(x + 1, y).a != 0)
        {
            return true;
        }

        if (x >= 1 && t.GetPixel(x - 1, y).a != 0)
        {
            return true;
        }

        if (y >= 1 && t.GetPixel(x, y - 1).a != 0)
        {
            return true;
        }

        if (y < t.height - 1 && t.GetPixel(x, y + 1).a != 0)
        {
            return true;
        }

        if (diagonal)
        {
            if (x < t.width - 1)
            {
                if (y < t.height - 1 && t.GetPixel(x + 1, y + 1).a != 0)
                {
                    return true;
                }

                if (y >= 1 && t.GetPixel(x + 1, y - 1).a != 0)
                {
                    return true;
                }
            }
            if (x >= 1)
            {
                if (y >= 1 && t.GetPixel(x - 1, y - 1).a != 0)
                {
                    return true;
                }

                if (y < t.height - 1 && t.GetPixel(x - 1, y + 1).a != 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
