   M S B A M L   `   `   `  ���� C  ?Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1      WpfApp1.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:WpfApp1       � WindowStyle$
  None=� � 
ResizeMode$ NoResize=� � AllowsTransparency . 
������  � Title$ 
MainWindow��$	��566��$	��970��.��� -   .+��� $��/Resources/Background.png��$
��Fill=��� .+��� -   $��Start�� J� Opacity$ 0Q� � Cursor$
 Hand��$
��Left=� �� Bottom$ 0��$	��Top=�$��50��$	��175�� �� Left$	 690��
 �� Top$	 444���� -   $ 0Q�$
 Hand��$��Center=�$��Center=�$��52��$��42��$	 148��$ 58���� -   $��Mimized��$
 Hand��$ 0Q�$��Center=�$ 0��$��Center=�$��53��$��39��$	 858��$ -3���� -   $��Close��$ 0Q�$
 Hand��$
��Left=�$��Center=�$��41��$��40��$	 909���� -   	 ��Name$	 Setting��$
 Hand��$ 0Q�$
��Left=�$	��Top=�$��47��$��40��$	 454��$ 63��� -   
 �Name$
 totalProgressBar�� � Minimum$ 0Q� � Maximum$	 100Q�$��14��
������ 
������ $	��Top=�$	��523��$	 111��$	 488��$��Center=�����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,0&�.���  � Color$ #64472C�� � Offset$ 0Q�� $ #D37C1F��$ 1Q�� -   $
 updateProgressBar��$ 0Q�$	 100Q�$��16��
������ 
������ $	��523��$	 111��$	 444��$
��Left=�$��Center=����� $	 0,0&�$	 1,0&�.��� $ #775C36��$ 0Q�� $ #FFFFFF��$ 1Q��� -	    ��Name$ windowModeCheckBox��$
��Left=�$	��Top=� J� RenderTransformOrigin$ 0.751,2.917&�$	 717��$	 512�� d� 	IsChecked ��  �� Path$ 
WindowMode��� $��Window Mode��
�������$	 733��$	 505��$��Center=�$	��Top=�$��Arial)��� -
   $��status�� �� TextAlignment$ Center=�$	��532��$��30��
�������$��16(�$	 102��$	 410��$
��Left=�$��Arial)�$��Center=��� -   $��
serverTime��
�������$��Center=�$��Center=�$	��208��$��25��$��16(�$	 111��$	 507���� -   $��connectServer��$
��Left=�$	��Top=�$��16(�$	��102��$��39��$	 102��$	 292���� -   $��
gameServer��$
��Left=�$	��Top=�$��16(�$	��102��$��30��$	 111��$	 372���� -   $��TotalProgressText��$�� ��$
��Bold"�$	 374��$	 488��$��Center=�$	��Top=����	#FFF3EEE9$��Arial)� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     Color$ Black��   
BlurRadius$ 3Q�   ShadowDepth$ 2Q�   Opacity$	 0.7Q��� -   $��UpdateProgressText��$�� ��$
��Bold"�$	 374��$	 445��$
��Left=�$	��Top=����	#FFF3EEE9$��Arial)�   $ Black��$ 3Q�$ 2Q�$	 0.7Q�