   M S B A M L   `   `   `  ���� H  DTetrisWithWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:TetrisWithWPFTetrisWithWPF      TetrisWithWPF.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:TetrisWithWPF     -     � Title$  Tetris-Space��$	��600��$	��800��$	��600��$	��600��
�������$��Andale mono)�$��30(�.���  � RowDefinitions �� $
��auto ��� $��* � � ColumnDefinitions �� $��* ��� $
��auto ��� $��* �����  �� ImageSource$, &Assets/Dall-E-Generated-Background.png��.+�(� $��1��$��1��$��0,0,0,20q�.���� -    ��Name$ 
GameCanvas��$��1��$��1�����	#CC000000$	��250��$	��510��$
}�True���� -    ��Name$ 	ScoreText��$��0��$��1��$��Current Score:��$��0, 10q� �� TextAlignment$ Center=��� $��1��$��0��$��Center=�$��Right=�.+��� $��
Hold Block��$ Center=��� -    ��Name$ 	HoldImage��$��20q�$	��125���� $��1��$��2��$��Center=�$
��Left=�.+��� $��
Next Block��$ Center=��� -   $
 Next��$��20q�$	��125��� -    �Name$ GameOver�����	#CC000000$��2��$��3��$y�Hidden=�.+��� $��Center=�$��Center=�.+��� $��	Game over��$��38(�$ Center=�
��������� -   $ TheFinalScoreText��$��Score:��$��30(�$ Center=�
��������� -   $��	Try Again��
����� �$��0, 20, 0, 0q�$��7q�