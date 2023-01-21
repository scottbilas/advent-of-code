using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;
using System.IO;

public class Day18 : MonoBehaviour
{
    public GameObject CubeOn, CubeOff;
    public bool Part1;

    void Start()
    {
        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        var inputTxt = File.ReadAllText("../aoc/day18.input.txt");
        var coords = Regex.Matches(inputTxt, @"\d+").Select(m => int.Parse(m.Value)).ToArray();

        for (var i = 0; i < coords.Length; i += 3)
        {
            var cube = Instantiate(CubeOn);
            cube.transform.position = new Vector3(coords[i], coords[i + 1], coords[i + 2]);

/*            var color = cube.GetComponent<Renderer>().material.color;
            color.r = Random.Range(0f, 1f);
            color.g = Random.Range(0f, 1f);
            color.b = Random.Range(0f, 1f);
            color.a = 1f;
            cube.GetComponent<Renderer>().material.SetColor("_Color", color);*/

            if (i % 30 == 0)
                yield return new WaitForSeconds(.05f);
        }
    }
}
