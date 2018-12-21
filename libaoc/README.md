### Stuff to make

* Simple linear repeating pattern detector
  * Feed objects of any type, but at least string and int
  * Ability to project forward to return sequence at [offset, count]
* Smart grid object
  * Chars and objects, filling, cloning
  * Parsing (with detection for leaders), auto trimming, auto bounds detecting
  * Path finding built-in and ability to say what is blocker or not
  * Flood-fill algorithms
  * Rendering to png or animation
  * Offset ability (can use (123,234) in [10,10] grid) -- use Rect bounds for this, then easy test for inbounds and also keep cx/cy
  * Grow ability, maybe via 100x100 chunking..maybe just make that the main type
  * ^ most of these should be extension methods on char/object[,] and give grid ability to implicit cast to that
  * ^ or use a new interface type I2DGrid<T>...
  * grid access[,] with Point obj
* Sparse grid object
* IGrid for algorithms (implemented by sparse and smart grids)
  * line and box drawing
* Rectangle missing common stuff like TopLeft() - could go with extension methods but maybe just find a nuget package or write one for 2d
* And Rectangle.Zero, Point.Zero..
* Deque that impls IReadOnlyList, with pythonisms like rotate, and ability to set max size which will truncate from the other side when adding too much to one side
* Reverse() that works correctly on IReadOnlyList

Worth doing unit tests for all these..
