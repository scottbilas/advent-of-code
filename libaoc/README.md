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

Worth doing unit tests for all these..
