   M S B A M L   `   `   `  ����K  GCourseProjectOOP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:CourseProjectOOP.View.WindowsCourseProjectOOP.View.Windows  5   	   -    'CourseProjectOOP.View.Windows.CutWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =local+clr-namespace:CourseProjectOOP.View.Windows     5   	     � Title$  	CutWindow��5   	   $	��450��6   $	��800��6(   .��5	      �    ScrollingAreaCol#���   6    � ColumnDefinitions 5
   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      �� 5      $
��0.4* �6   �� 5      $
��0.2* �6   6,   .+�5   
   �� -   5         ��Name$ NewBeginning��5   6   $��1��6   $��80��6   $��20��6*   6o   �� 5   
   -   5        $ 	NewEnding��5   F   $��1��6   $��1��6   $��80��6/   $��20��6:   6y   �� 5   
   -   5         ��Name$ 	CutButton��5      $��Обрезать��6$   $��70��67   $��30��6B   $��2��6N   $��2��6[   6�   �� 5   
   -   5        $ PointButton��5      $��
Время��6&   $��70��66   $��30��6A   $��1��6M   $��2��6Z   6�   �� 5   
   $��Bottom=�6   $��2��60   .+�5      r� -   5         r�Name$ 	VideoShow��5      $	��500��6/   $	��250��6;   $��0 0 0 0q�6H    r� LoadedBehavior$ Manual=�6Y   6q   �� 5      -   5         ��Name$ VideoPosition��5      $��2��6,   $	��500��6@   6�   5      �� 5   
   -   5        $ PauseButton��5      $��50��6&   $��50��61   $
��Left=�6=   $��50 0 0 0q�6X   $	��⏯��6j   $��30(�6v   6�   �� 5   
   -   5        $ ProcessStopButton��5      $��П��6,   $��30��68   $��30��6C   $��2��6O   $��16,12,314,12q�6|   5      5      