package libaoc

type StackInt struct {
	stack []int
}

func (stack *StackInt) Push(value int) { stack.stack = append(stack.stack, value) }
func (stack *StackInt) Pop()           { stack.stack = stack.stack[:len(stack.stack)-1] }
func (stack *StackInt) Front() int     { return stack.stack[len(stack.stack)-1] }
func (stack *StackInt) Size() int      { return len(stack.stack) }
func (stack *StackInt) Empty() bool    { return len(stack.stack) == 0 }
