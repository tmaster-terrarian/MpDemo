using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MpDemo.Client;

public class Player
{
    private Point _position;
    private Vector2 _velocity;

    public Point Position => _position;
    public Vector2 Velocity => _velocity;

    public Player()
    {
        _position.X = Random.Shared.Next(MainWindow.Width + 1);
        _position.Y = Random.Shared.Next(MainWindow.Height + 1);
    }
}
