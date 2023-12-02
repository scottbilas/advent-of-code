using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;
using System.IO;

public class Day22 : MonoBehaviour
{
    public GameObject CubeOn, CubeOff;
    public bool Part1;

    void Start()
    {
        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        var inputTxt = File.ReadAllText("../aoc/day22.input.txt");
        var onoff = Regex.Matches(inputTxt, @"on|off");
        var coords = Regex.Matches(inputTxt, @"-?\d+").Cast<Match>().Select(m => int.Parse(m.Value)).ToArray();

        for (var ib = 0; ib < onoff.Count; ++ib)
        {
            var on = onoff[ib].Value == "on";
            var c = coords[(ib * 6)..(ib * 6 + 6)];

            if (Part1 && c.Any(c => c < -50 || c > 50))
                continue;

            var cube = Instantiate(on ? CubeOn : CubeOff);
            var mesh = cube.GetComponent<MeshFilter>().mesh;
            var vertices = mesh.vertices;

            for (var iv = 0; iv < vertices.Length; ++iv)
            {
                vertices[iv].x = vertices[iv].x == -.5 ? c[0] : c[1];
                vertices[iv].y = vertices[iv].y == -.5 ? c[2] : c[3];
                vertices[iv].z = vertices[iv].z == -.5 ? c[4] : c[5];
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            var color = cube.GetComponent<Renderer>().material.color;
            color.a = Random.Range(0f, 1f);
            cube.GetComponent<Renderer>().material.color = color;

            yield return new WaitForSeconds(.05f);
        }
    }
}
