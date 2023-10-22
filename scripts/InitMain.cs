using Godot;
using System;

public partial class InitMain : Node3D
{
	[Export]
	PackedScene mainNivel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //InitFullScreen();
        CaptureMouse();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Input_Exit_Game();
        Input_Change_Level();
    }


    private void CaptureMouse()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        Input.MouseMode = Input.MouseModeEnum.Hidden;
    }


    private void InitFullScreen()
    {
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
    }

    private void Input_Change_Level()
    {
        if (Input.IsActionJustPressed("space"))
        {
            GetTree().ChangeSceneToPacked(mainNivel);
        }

    }

    private void Input_Exit_Game()
    {
        if (Input.IsActionJustPressed("escape"))
        {
            GetTree().Quit();
        }
    }
}
