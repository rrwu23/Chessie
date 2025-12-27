using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


public class Node
{
    public List<Node> ChildNodes { get; set; }
    public object? Content { get; set; } //need nullable type

    // Constructor with content and children
    public Node(object content, List<Node> nodes)
    {
        Content = content;
        ChildNodes = nodes;
    }

    // Constructor with content only (leaf node)
    public Node(object content)
    {
        Content = content;
        ChildNodes = new List<Node>();
    }

    // Default constructor
    public Node()
    {
        ChildNodes = new List<Node>();
    }
}

public class Tree
{
    public Node InitialNode { get; set; }

    public Tree(Node root)
    {
        InitialNode = root;
    }
}

public class Program
{
    public static void Main()
    {
        /*var child1 = new Node("e4");
        var child2 = new Node("ke2");

        var root = new Node("lol", new List<Node> { child1, child2 });

        var myTree = new Tree(root);

        Console.WriteLine($"Tree root has {myTree.InitialNode.ChildNodes.Count} child nodes.");
        Console.WriteLine($"Root content: {myTree.InitialNode.Content}");*/

        //loop
        ChessBoard delaboard = new ChessBoard(new List<List<Piece?>>()
        {
            new List<Piece?>() //a
            {
                new Piece('r', "a8", -1),
                new Piece('n', "b8", -1),
                new Piece('b', "c8", -1),
                new Piece('q', "d8", -1),
                new Piece('k', "e8", -1),
                new Piece('b', "f8", -1),
                new Piece('n', "g8", -1),
                new Piece('r', "h8", -1),
            },
            new List<Piece?>() //b
            {
                new Piece('p', "a7", -1),
                new Piece('p', "b7", -1),
                new Piece('p', "c7", -1),
                new Piece('p', "d7", -1),
                new Piece('p', "e7", -1),
                new Piece('p', "f7", -1),
                new Piece('p', "g7", -1),
                new Piece('p', "h7", -1),
            },
            new List<Piece?>() //c
            {
                null, null, null, null, null, null, null, null
            },
            new List<Piece?>() //d
            {
                null, null, null, null, null, null, null, null
            },
            new List<Piece?>() //e
            {
                null, null, null, null, null, null, null, null
            },
            new List<Piece?>() //f
            {
                null, null, null, null, null, null, null, null
            },
            new List<Piece?>() //g
            {
                new Piece('p', "a2", 1),
                new Piece('p', "b2", 1),
                new Piece('p', "c2",1),
                new Piece('p', "d2", 1),
                new Piece('p', "e2", 1),
                new Piece('p', "f2", 1),
                new Piece('p', "g2", 1),
                new Piece('p', "h2",  1),
            },
            new List<Piece?>() //h
            {
                new Piece('r', "a1", 1),
                new Piece('n', "b1", 1),
                new Piece('b', "c1", 1),
                new Piece('q', "d1", 1),
                new Piece('k', "e1", 1),
                new Piece('b', "f1", 1),
                new Piece('n', "g1", 1),
                new Piece('r', "h1",  1),
            },
        });

        delaboard.InitializeStorage();

        while (true)
        {
            string? command = Console.ReadLine();
            if (command == "quit")
            {
                Console.WriteLine("chess or stress");
                return;
            }

            else if (command == "showboard")
            {
                Console.WriteLine(delaboard.showboard());
            } else
            {
                delaboard.Execute(command ?? "");
            }


            
        }

    }
}

public class Piece(char? ptype, string? position, int mover)
{
    public char? P_type = ptype;
    public string? Position = position;
    public int? Mover = mover;
}

public class ChessBoard(List<List<Piece?>> board)
{
    public List<List<Piece?>> Board = board;

    public List<Piece>? P_Storage;
    public List<char> letters =
        new List<char> {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'}
        ;
    
    public List<int> numbers =
        new List<int> {1, 2, 3, 4, 5, 6, 7, 8}
        ;

    public double Eval()
    {
        double _evalw = 0;
        double _evalb = 0;

        //code over here


        return _evalw - _evalb;
    }

    public void InitializeStorage()
    {
        P_Storage = Board.SelectMany(p => p)
        .Where(pi => pi is not null)
        .Select(pi => pi!)
        .ToList();
    }

    public void Execute(string moveNotation)
    {
        if (! Regex.IsMatch(moveNotation, @"^[nbrq]?[a-h1-8]?[a-h]x?[1-8][+#]?$"))
        {
            if (moveNotation == "O-O" || moveNotation == "O-O-O")
            {
                Console.WriteLine("castle not yet supported");
                return;
            }
            Console.WriteLine("invalid move notation");
            return;
        }
        char piece = moveNotation[0];
        string mtype = "normal";
        string position = moveNotation[1..];
        Console.WriteLine("position: " + position);

        Console.WriteLine(piece);
        Console.WriteLine(position.Length);

        if (moveNotation[1] == 'x') //
        {
            mtype = "capture";
            position = moveNotation[2..]; //
        }

        // if (moveNotation[moveNotation.Count() - 1] == '+' || moveNotation[moveNotation.Count() - 1] == '#')
        // {
            
        // }

        if (position.Length == 1)
        {
            Console.WriteLine("TRIGGERED");
            piece = 'p';
            position = moveNotation;
        }
        Console.WriteLine(position);
        Console.WriteLine(piece);
        Console.WriteLine(position.Length);

        var avap = Board.SelectMany(p => p)
        .Where(pi => pi?.P_type == piece);

        foreach (var p in avap)
        {
            Console.WriteLine("aviaible piece detected");
            //check if move is valid
            if (IsAvailiableMove(p?.Position, position, p?.P_type, mtype, p?.Mover))
            {
                p?.Position = position;
                updateBoard();
                break;
            }
        }
    }

    public void updateBoard()
    {
        //rebuild board from piece positions
        //list to store all the pieces
        List<List<Piece?>> newboard = new List<List<Piece?>>();
        for (int c = 0; c < 8; c++)
        {
            List<Piece?> board = new List<Piece?>();
            for (int r = 0; r < 8; r++)
            {
                board.Add(null);
            }
            newboard.Add(board);
        }

        Console.WriteLine(P_Storage);
        for (int c = 7; c >= 0; c--)
        {
            for (int r = 0; r < 8; r++)
            {
                foreach (var p in P_Storage!)
                {
                    //Console.WriteLine(string.Join(", ", P_Storage));
                    if (p.Position != null && p.Position == $"{letters[r]}{numbers[c]}")
                    {
                        Console.WriteLine($"{r}, {c}");
                        Console.WriteLine("yeepee");
                        newboard[c][r] = p;
                        break;
                    } else
                    {
                        newboard[c][r] = null;
                    }
                }
            }
        }

        Board = newboard;
    }

    public bool IsAvailiableMove(string? s_pos, string t_pos, char? p_type, string m_type, int? mover)
    {
        /*
        * spos = start position
        * tpos = target position
        * ptype = piece type
        * mtype = move type
        * mover = which player is moving (1 = white, -1 = black)
        */

        if (s_pos is null || p_type is null || mover is null)
        {
            return false;
        }

        int s_col = letters.IndexOf(s_pos[0]);
        int s_row = numbers.IndexOf(int.Parse(s_pos[1].ToString())); 
        int t_col = letters.IndexOf(t_pos[0]);
        int t_row = numbers.IndexOf(int.Parse(t_pos[1].ToString()));

        switch (p_type)
        {


            //pawn
            case 'p':
                if (m_type == "capture")
                {
                    Console.WriteLine("capture");
                    //diagonal capture logic
                    //e5 - f6
                    //oh my god wh
                    

                    if (Math.Abs(s_col - t_col) == 1 && (s_row - t_row) * mover == 1)
                    {
                        // Diagonal capture is valid
                        Console.WriteLine("ok");
                        return true;
                    } else
                    {
                        return false;
                    }

                } else
                {

                    //how in the world
                    if (s_col == t_col)
                    {
                        Console.WriteLine("check passed");
                        if (Math.Abs(s_row - t_row) == 2 && s_row == 1)
                        {
                            Console.WriteLine("w=kinda worked");
                            return true;
                        }
                        else
                        {
                            if (Math.Abs(s_row - t_row) == 1)
                            {
                                Console.WriteLine("w=kinda worked");
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    } else
                    {
                        return false;
                    }
                }

            case 'r':
                //rook move logic
                // (same row or same column)
                // work on this 
                if (s_col == t_col)
                {
                    int side = (t_row - s_row)/Math.Abs(s_row-t_row);
                    for (int prog = 1; prog <= Math.Abs(s_row - t_row); prog++) //vertical/row
                    {
                        if (Board[s_row + side*prog][s_col] != null)
                        {
                            Console.WriteLine("inbetween");
                            return false;
                        }
                    }
                    return true;
                } else if (s_row == t_row)
                {
                    int side = (s_col - t_col)/Math.Abs(s_col-t_col);
                    for (int prog = 1; prog <= Math.Abs(s_col - t_col); prog++)
                    {
                        if (Board[s_row][s_col - side*prog] != null)
                        {
                            Console.WriteLine("inbetween");
                            return false;
                        }
                    }

                    return true;
                } else
                {
                    return false;
                }

            case 'n':
                //knight move logic
                // +2 || -2 && +1 || -1 || +1 || -1 && +2 || -2
                if ((Math.Abs(t_col - s_col) == 2 && Math.Abs(s_row - t_row) == 1) ||
                    (Math.Abs(s_col - t_col) == 1 && Math.Abs(s_row - t_row) == 2))
                {
                    return true;
                } else
                {

                    return false;
                }

            case 'b':
                //bishop move logic
                // math.absscol - tcol == srow - trow 
                if (Math.Abs(s_col - t_col) == Math.Abs(s_row - t_row)) //wait what
                {
                    int cside = (t_col-s_col)/Math.Abs(t_col-s_col);
                    int rside = (t_row-s_row)/Math.Abs(t_row-s_row);
                    Console.WriteLine("work with " + s_pos);
                    //piece block
                    for (int prog = 1; prog <= Math.Abs(s_col - t_col); prog++)
                    {
                        if (Board[s_row+rside*prog][s_col+cside*prog] != null)
                        {
                            Console.WriteLine("piece inbetween");
                            return false;
                        }
                    }

                    return true;
                    
                } else
                {
                    return false;
                }
                
            case 'q':
                //queen move logic
                //rook + bishop
                if (s_col == t_col || s_row == t_row ||
                    Math.Abs(s_col - t_col) == Math.Abs(s_row - t_row))
                {
                    return true;
                } else
                {
                    return false;
                }

            case 'k':
                //king move logic
                //one square any direction
                // +1 || -1 || 0 && +1 || -1 || 0
                if (Math.Abs(s_col - t_col) <= 1 && Math.Abs(s_row - t_row) <= 1)
                {
                    if (s_col == t_col && s_row == t_row)
                    {
                        return false;
                    }
                    return true;
                } else
                {
                    return false;
                }

            
            default:
            return false;
        }

        //return false;
    }
 
    public string showboard()
    {
        string boardStr = "";

        for (int r = 7; r>=0; r--) //top
        {
            for (int c = 0; c < 8; c++) //top
            {
                var piece = Board[r][c];
                if (piece is null)
                {
                    boardStr += ". ";
                } else
                {
                    boardStr += piece.P_type + " ";
                }
            }
            boardStr += "\n";
        }

        return boardStr;
    }

    //public bool 
}


