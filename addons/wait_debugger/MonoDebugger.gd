tool
extends EditorPlugin

var btn: CheckBox = null	
const setting = 'mono/debugger_agent/wait_for_debugger'
var isOn: bool

func _enter_tree() -> void:
	isOn = ProjectSettings.get_setting(setting)
	btn = CheckBox.new()
	btn.text = "Wait for debugger?"
	btn.pressed = isOn
	btn.connect('pressed', self, 'on_button_pressed')
	add_control_to_container(CONTAINER_TOOLBAR, btn)
	

func _exit_tree() -> void:
	btn.queue_free()
	remove_control_from_container(CONTAINER_TOOLBAR, btn)
	btn = null
	
func on_button_pressed() -> void:
	isOn = !isOn
	print(isOn)
	ProjectSettings.set_setting(setting, isOn)
	ProjectSettings.save()
