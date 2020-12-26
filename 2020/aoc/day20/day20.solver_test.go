package main

import (
	"fmt"
	"math"
	"regexp"
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = ReadSampleFile()
var input = ReadInputFile()

var seaMonster = [3]string{
	"                  # ",
	"#    ##    ##    ###",
	" #  #  #  #  #  #   "}

func makeSeaMonsterRx(dim int) *regexp.Regexp {
	pattern := ""
	for i, line := range seaMonster {
		text := "(" + strings.ReplaceAll(line, " ", ".") + ")"
		if i != len(seaMonster)-1 {
			text += fmt.Sprintf(".{%d}", dim-len(line))
		}
		pattern += text
	}

	return regexp.MustCompile(pattern)
}

type Image struct {
	tileDim  int // dim in cells
	imageDim int // dim in tiles

	tiles       map[int][]byte // id -> bits
	edgesByBits map[int][]int  // bits -> ids
	edgesByTile map[int][8]int // id -> bits
	corners     [4]int         // id
}

func makeImage(source string) Image {
	var image Image

	// detect tiles and dimensions
	blocks := ParseBlocks(source)
	image.tileDim = len(ParseLines(blocks[0])[1])
	image.imageDim = int(math.Sqrt(float64(len(blocks))))

	// copy tiles
	image.tiles = make(map[int][]byte)
	for _, block := range blocks {
		lines := ParseLines(block)
		id := ParseInts(lines[0])[0]
		tile := make([]byte, image.tileDim*image.tileDim)
		for cy := 0; cy < image.tileDim; cy++ {
			for cx := 0; cx < image.tileDim; cx++ {
				tile[image.offset(cx, cy)] = lines[cy+1][cx]
			}
		}
		image.tiles[id] = tile
	}

	image.edgesByBits = make(map[int][]int)
	image.edgesByTile = make(map[int][8]int)

	// index edges
	for id := range image.tiles {
		for _, bits := range image.updateEdges(id) {
			image.edgesByBits[bits] = append(image.edgesByBits[bits], id)
		}
	}

	// find corners
	corners := 0
	for id, edges := range image.edgesByTile {
		count := 0
		for i := 0; i < 4; i++ {
			count += ToInt(len(image.edgesByBits[edges[i]]) == 2)
		}
		if count == 2 {
			image.corners[corners] = id
			corners++
		}
	}

	return image
}

func (image *Image) updateEdges(id int) [8]int {
	tile := image.tiles[id]

	getEdge := func(offset func(i int) int, reverse bool) int {
		bits := 0
		for i := 0; i < image.tileDim; i++ {
			if tile[offset(i)] == '#' {
				shift := i
				if reverse {
					shift = image.reverse(shift)
				}
				bits |= 1 << shift
			}
		}

		return bits
	}

	edges := [8]int{
		getEdge(func(i int) int { return image.offset(i, 0) }, true),                 // 0 = top:    left   -> right
		getEdge(func(i int) int { return image.offset(image.reverse(0), i) }, true),  // 1 = right:  top    -> bottom
		getEdge(func(i int) int { return image.offset(i, image.tileDim-1) }, false),  // 2 = bottom: right  -> left
		getEdge(func(i int) int { return image.offset(0, i) }, false),                // 3 = left:   bottom -> top
		getEdge(func(i int) int { return image.offset(0, i) }, true),                 // 4 = left:   top    -> bottom
		getEdge(func(i int) int { return image.offset(i, image.tileDim-1) }, true),   // 5 = bottom: left   -> right
		getEdge(func(i int) int { return image.offset(image.reverse(0), i) }, false), // 6 = right:  bottom -> top
		getEdge(func(i int) int { return image.offset(i, 0) }, false),                // 7 = top:    right  -> left
	}

	image.edgesByTile[id] = edges
	return edges
}

func reverse(pos, dim int) int           { return dim - 1 - pos }
func (image *Image) reverse(pos int) int { return reverse(pos, image.tileDim) }
func offset(x, y, dim int) int           { return y*dim + x }
func (image *Image) offset(x, y int) int { return offset(x, y, image.tileDim) }

func flip(grid []byte, dim int) {
	for y := 0; y < dim; y++ {
		for x := 0; x < dim/2; x++ {
			o1, o2 := offset(x, y, dim), offset(reverse(x, dim), y, dim)
			grid[o1], grid[o2] = grid[o2], grid[o1]
		}
	}
}

func (image *Image) flip(id int) {
	flip(image.tiles[id], image.tileDim)
	image.updateEdges(id)
}

func rotate(grid []byte, dim int) []byte {
	dst := make([]byte, dim*dim)
	for y := 0; y < dim; y++ {
		for x := 0; x < dim; x++ {
			dst[offset(reverse(y, dim), x, dim)] = grid[offset(x, y, dim)]
		}
	}
	return dst
}

func (image *Image) rotate(id int) {
	image.tiles[id] = rotate(image.tiles[id], image.tileDim)
	image.updateEdges(id)
}

// PART 1

func solve1(source string) int {
	image := makeImage(source)
	return image.corners[0] * image.corners[1] * image.corners[2] * image.corners[3]
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, func() int { return solve1(sample) }, 20899048083289)
	AssertEqualSub("Problem", t, solve1(input), 27798062994017)
}

// PART 2

func solve2(source string) int {
	image := makeImage(source)

	getLink := func(fromId, fromEdge int) (id, bits int) {
		bits = image.edgesByTile[fromId][fromEdge]
		matches := image.edgesByBits[bits]
		if len(matches) == 2 {
			id = matches[0]
			if id == fromId {
				id = matches[1]
			}
		}
		return
	}

	hasLink := func(fromId, fromEdge int) bool {
		link, _ := getLink(fromId, fromEdge)
		return link != 0
	}

	// move first corner into upper left
	current := image.corners[0]
	for i := 0; hasLink(current, 0) || hasLink(current, 3); i++ {
		image.rotate(current)
	}

	// put together the image
	ordered := make([]int, image.imageDim*image.imageDim)
	for y := 0; y < image.imageDim; y++ {
		for x := 0; x < image.imageDim; x++ {

			// skip the corner we are starting with
			if x == 0 && y == 0 {
				ordered[0] = current
				continue
			}

			// figure out edge to match (right<>left normally, bottom<>top on next row)
			basis, fromEdge, toEdge := current, 1, 4
			if x == 0 {
				basis, fromEdge, toEdge = ordered[offset(0, y-1, image.imageDim)], 5, 0
			}

			// find matching tile
			var bits int
			current, bits = getLink(basis, fromEdge)
			ordered[offset(x, y, image.imageDim)] = current

			// get it into position
			for i := 0; image.edgesByTile[current][toEdge] != bits; i++ {
				image.rotate(current)
				if i == 3 {
					image.flip(current)
				}
			}
		}
	}

	innerDim := image.tileDim - 2
	outerDim := image.imageDim * innerDim
	tiled := make([]byte, outerDim*outerDim)

	// assemble the image so we can regex it
	for ty, toff := 0, 0; ty < image.imageDim; ty++ {
		for tx := 0; tx < image.imageDim; tx, toff = tx+1, toff+1 {
			tile := image.tiles[ordered[toff]]
			for y := 0; y < innerDim; y++ {
				od, os := offset(tx*innerDim, ty*innerDim+y, outerDim), image.offset(1, y+1)
				copy(tiled[od:od+innerDim], tile[os:os+innerDim])
			}
		}
	}

	// find sea monsters
	seaMonsterRx := makeSeaMonsterRx(outerDim)
	for i := 0; ; i++ {
		bytes, found := tiled, false

		// golang rx doesn't do overlaps, so do it manually
		for pos := 0; ; {
			match := seaMonsterRx.FindSubmatchIndex(bytes[pos:])
			if match == nil {
				break
			}

			// draw sea monster
			for i, line := range seaMonster {
				m := match[(i+1)*2]
				for j, c := range line {
					if c == '#' {
						bytes[pos+m+j] = 'O'
					}
				}
			}

			pos = match[0] + 1
			found = true
		}

		// sea monsters means we have an answer
		if found {
			hashes := 0
			for _, c := range bytes {
				if c == '#' {
					hashes++
				}
			}
			return hashes
		}

		// otherwise continue to rotate and flip
		tiled = rotate(tiled, outerDim)
		if i == 3 {
			flip(tiled, outerDim)
		}
	}
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, func() int { return solve2(sample) }, 273)
	AssertEqualSub("Problem", t, func() int { return solve2(input) }, 2366)
}
