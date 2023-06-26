tool
extends EditorPlugin


func _enter_tree():
	add_custom_type("BEH_Tree Controller", "Node", preload("res://Scripts/Behavior Tree/Base/TreeController.cs"), null)
	add_custom_type("BEH_Composite Node", "Node", preload("res://Scripts/Behavior Tree/Base/CompositeNode.cs"), null)
	add_custom_type("BEH_Sequence Node", "Node", preload("res://Scripts/Behavior Tree/Base/SequenceNode.cs"), null)
	add_custom_type("BEH_Fallback Node", "Node", preload("res://Scripts/Behavior Tree/Base/FallbackNode.cs"), null)
	add_custom_type("BEH_Decorator Node", "Node", preload("res://Scripts/Behavior Tree/Base/DecoratorNode.cs"), null)
	add_custom_type("BEH_1_Console", "Node", preload("res://Scripts/Behavior Tree/Base/WriteConsole.cs"), null)
	
func _exit_tree():
	remove_custom_type("BEH_Tree Controller")
	remove_custom_type("BEH_Composite Node")
	remove_custom_type("BEH_Sequence Node")
	remove_custom_type("BEH_Fallback Node")
	remove_custom_type("BEH_Decorator Node")
	remove_custom_type("BEH_1_Console")
	
