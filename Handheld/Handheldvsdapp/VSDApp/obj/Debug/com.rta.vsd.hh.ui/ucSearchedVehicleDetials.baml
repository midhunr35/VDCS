   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      7    1VSDApp.com.rta.vsd.hh.ui.ucSearchedVehicleDetials   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      6
propertiesclr-namespace:VSDApp.Properties     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        
�������$
��Auto��5   K   $
��Auto��6X     � 	Resources  5	       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      �� $
��Auto=�6   $
��Auto=�66   .��5   
   � -   5         �Name$ MainGrid��5       � RowDefinitions 5      �� 5      $��* �6    �� 5      $
��Auto �6    �� 5      $��10 �6    �� 5      $
��Auto �6    �� 5      $��10 �6    �� 5      $
��Auto �6    �� 5      $��10 �6    �� 5      $
��Auto �6    �� 5       $��10 �6    �� 5!      $
��Auto �6    �� 5"      $��10 �6    �� 5#      $
��Auto �6    �� 5$      $��10 �6    �� 5%      $
��Auto �6    �� 5&      $��10 �6    �� 5'      $
��Auto �6    �� 5(      $��10 �6    �� 5)      $
��Auto �6    �� 5*      $��10 �6    �� 5+      $
��Auto �6    �� 5,      $��10 �6    �� 5-      $
��Auto �6    �� 5.      $��10 �6    �� 5/      $
��Auto �6    �� 50      $��5 �6    �� 51      $��* �6     � ColumnDefinitions 54      �� 55      $��* �6#   �� 56      $
��Auto �6#   �� 57      $��5 �6#   �� 58      $
��Auto �6#   �� 59      $��5 �6#   �� 5:      $
��Auto �6#   �� 5;      $��5 �6#   �� 5<      $
��Auto �6#   �� 5=      $��* �6#   .+�5?      �� -   5         ��Name$ lblVehDetail��5?      !   VSDApp.Properties.Resources   VehicleDetials#��Z 6*   $��Segoe UI)�6e   $��Medium"�6{   $��30(�6�   $��1��6�   $��1��6�   $��3��6�   $��Stretch=�6�   $��Stretch=�6�   �� 5@      -   5        $ 
lblOprName��5@      $
��Left=�6(   $��3��6D   $��1��6Q      OperatorName#��Z 6a   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5A      -   5        	 ��Name$	 txtBoxOperatorName��5A      $��Bottom=�63   $��3��6N   $��3��6[      TextBoxLarge#��[  6k   $
��Left=�6�   
 �� 
IsReadOnly
 .  6D  �� 5B      -   5        $ lblChassisName��5B      $
��Left=�6,   $��3��6G   $��5��6T      ChassisNumber#��Z 6d   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5C      -   5        $	 txtBoxChassisNumber��5C      $��Bottom=�63   $��3��6N   $��7��6[   #��[  6k   $
��Left=�6�   
 .  6D  �� 5D      -   5        $ lblMake��5D      $
��Left=�6%   $��5��6@   $��1��6M      Make#��Z 6]   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5E      -	   5        $	 
txtBoxMake��5E      $��5��6*   $��Bottom=�67   $��3��6R   #��[  6b   $
��Left=�6�   
 .  6;  �� 5F      -
   5        $ lblModel��5F      $
��Left=�6&   $��5��6A   $��5��6N      Model#��Z 6^   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5G      -   5        $	 txtModel��5G      $��5��6(   $��7��65   $��Bottom=�6E   #��[  6`   $
��Left=�6�   
 .  69  �� 5H      -   5        $ lblYear��5H      $
��Left=�6%   $��7��6@   $��1��6M      Year#��Z 6]   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5I      -   5        $	 txtYear��5I      $��Bottom=�6(   $��7��6C   $��3��6P   #��[  6`   $
��Left=�6�   
 . 68  �� 5J      -   5        $ VehOVRR��5J      $
��Left=�6%   $��7��6@   $��3��6M   $��5��6a      VRRScore#��Z 6q   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5K      -   5        $	 txtOvrrScore2��5K      $��7��6-   $y�	Collapsed=�6:   $��Bottom=�6Q   $��7��6l   #��[  6|   $
��Left=�6�   
 . 6T  �� 5L      -   5         ��Name$ stckPanelVRR��5L      $��7��6/   $��7��6<   .+�5N      � -   5        $ 	grdcanvas��5N      $��10,0,0,0q�6*   $��7��6<   $��7��6I   $��50��6Z   $��40��6e   $��Center=�6q   .+�5O      �� -   5         ��Name$
 canv��5O      �� 5P      -   5        $ lblRattingTextwithColor��5P      $y�	Collapsed=�6=   $��G0��6T   $��17��6a   $��7��6o   $��Normal"�6   $��Center=�6�   
�������$��Center=�6�   $��Center=�6�   $��18(�6  5Q      5T      �� 5W      -   5        $ 	lblRecmnd��5W      $
��Left=�6'   $��9��6B   $��1��6O   $��3��6_   $��Segoe UI)�6s   $��Medium"�6�   $��20(�6�   �� 5Z      -   5        $ 	lblOpnVio��5Z      $
��Left=�6'   $��11��6B   $��1��6P      OpenViolations#��Z 6`   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   &   System.Windows.Controls.DataGrid  5[      -   5          Name$ grdViolationDetails��5[      $��11��64   $��3��6B   $��5��6R   $	��830��6f   $	��150��6r      AutoGenerateColumns .  $   HorizontalScrollBarVisibility$ Visible=�5\   4   $
��Left=�6\   ��. $��Visible=�5]   �   $
��Auto=�5^         CanUserAddRows .  $��Segoe UI)�6f   $��Medium"�6|   $��15(�6�   ��5]      �� *violationData+ �� UpdateSourceTrigger$ PropertyChanged=�   Columns 5_      4  .System.Windows.Controls.DataGridTemplateColumn  5`         BtnPrint,  &System.Windows.Controls.DataGridColumn   Header# Z 6-      
IsReadOnly .    
Visibility$ Visible=�6q      CellTemplate 5a      �� 5b      .R�5c   "   ��  ��Name$ btnPrintRecipt��6)   -   5        #��Z 5c   A     
ButtonGrid#��[ 6t   $y�Visible=�6�   $��Delete��6�   $��15(�6�   5d      5f      0  *System.Windows.Controls.DataGridTextColumn  5g      	   ID# Z 6)    .      Width5  /System.Windows.Controls.DataGridLengthConverter$
  Auto 6i   !   FontSize$! 15Q�6v   "   
FontFamily$" Segoe UI)�6�   #   
FontWeight$# Medium"�6�   1  +System.Windows.Controls.DataGridBoundColumn$   Binding$ 6�   �� *ViolationID+6�     5h      %   	IssueDate# Z% 6)    .  $
  Auto 6q   $! 15Q�6~   $" Segoe UI)�6�   $# Medium"�6�   $ 6�   �� *	IssueDate+6�     5i      # Z% 6)   $ 	Collapsed=�6\    .  $
  Auto 6�   $! 15Q�6�   $" Segoe UI)�6�   $# Medium"�6�   $ 6�   �� *	DueDate+6�     5j      &   Status# Z& 6)    .  $
  Auto 6n   $! 15Q�6{   $" Segoe UI)�6�   $# Medium"�6�   $ 6�   �� *ViolationStatus+6�     5k      '   Severity# Z' 6)    .  $! 15Q�6p   $" Segoe UI)�6~   $# Medium"�6�   $
  Auto 6�   $ 6�   �� *ViolationSeverity+6�     5l      (   PlateDetails# Z( 6)    .  $! 15Q�6s   $" Segoe UI)�6�   $# Medium"�6�   $
  Auto 6�   $ 6�   �� *VehicleDetail+6�     5m      )   ChassisNumber2# Z) 6)    .  $
  Auto 6u   $! 15Q�6�   $" Segoe UI)�6�   $# Medium"�6�   $ 6�   �� *ChassisNumber+6�     5n      *   MenueOperatorProfile# Z* 6*    .  $
  Auto 6{   $! 15Q�6�   $" Segoe UI)�6�   $# Medium"�6�   $ 6�   �� *OperatorDetail+6�   5r      �� 5t      -   5        $ btnStackePanel��5t      $��9��61   $��7��6>   + �� Orientation$+ 
Horizontal=�6N   $��Center=�6g   $��Right=�5u      .+�5v      �  5x      �� 5y      $��* �6(    5|      �� 5}      $��* �6+   �� 5~      $��* �6+   .+�5�      �� -   5        $ btnBack��5�      $��0��6.   $��0��6=   ,   BtnBack#��Z, 6M   $��0,0,0,0q�6     ButtonBackHalfControl#��[ 6�   $
��Left=�6�   6�   �� 5�      -   5        $ btnStartInspection��5�      $��0��69   $��1��6H   -   Inspect2#��Z- 6Y   $��10,0,0,0q�6�     ButtontoProceedHalfControl#��[ 6�   $��Right=�6�   6�   �� 5�      -   5        . ��Name$. imagebtnback��5�      $y�	Collapsed=�62   $&�� ..\Images\Buttons\Small\Back.png��6I   $	��140��6s   $��0 0 0 0q�6   $��Stretch=�6�   $��Stretch=�6�   6  �� 5�      -   5        $. imagebtnNext��5�      $y�	Collapsed=�62   $2��,..\Images\Buttons\Small\Start Inspection.png��6I   $	��140��6   $��30 0 0 0q�6�   $��Stretch=�6�   $��Stretch=�6�   6  5�      5�      �� 5�      $��Plate Confiscation:��6   $y�	Collapsed=�64   $��2��6K   $��20(�6_   $��0��6m   $��1��6z   $��Stretch=�6�   $��Stretch=�6�   �� 5�      $
��Left=�6   $y�	Collapsed=�63   $��2��6J   $��1��6W   $��Confiscation Required��6g   $��15(�6�   �� 5�      -   5        $	 txtIsPlateConfisReq��5�      $y�	Collapsed=�63   $��2��6J   $��3��6W   $	��300��6g   $��25��6s   $��Stretch=�6   $��Stretch=�6�   �� 5�      $
��Left=�6   $y�	Collapsed=�63   $��2��6J   $��5��6W   $��Plate Confiscated��6g   $��15(�6�   �� 5�      -   5        / ��Name$/ cmboxIsPlateConfiscated��5�      $y�	Collapsed=�68   $��2��6O   $��7��6\   $	��300��6l   $��25��6x   $��Stretch=�6�   $��Stretch=�6�   �� 5�      $
��Left=�6   $��4��63   $y�	Collapsed=�6@   $��5��6W   $��Plate Condition��6g   $��15(�6~   �� 5�      -   5        $/ cmboxPlateCndition��5�      $��4��63   $y�	Collapsed=�6@   $��7��6W   $	��300��6g   $��25��6s   $��Stretch=�6   $��Stretch=�6�   �� 5�      $
��Left=�6   $��4��63   $y�	Collapsed=�6@   $��1��6W   $��Confiscation Reason��6g   $��15(�6�   �� 5�      -    5        $	 txtReasonForConfiscation��5�      $��4��68   $y�	Collapsed=�6E   $��3��6\   $	��300��6l   $��25��6x   $��Stretch=�6�   $��Stretch=�6�   �� 5�      -!   5        $ btnNext��5�      $��10��6&   $��7��64   $
��Next��6D   $y�	Collapsed=�6S   $	��110��6j   $��25��6v   $��Right=�6�   5�      5�      5�      