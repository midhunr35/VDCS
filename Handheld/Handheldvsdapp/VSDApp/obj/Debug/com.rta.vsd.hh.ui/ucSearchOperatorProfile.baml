   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      Z VWPFToolkit.Extended, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4dEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.ExtendedMicrosoft.Windows.Controls 5      6    0VSDApp.com.rta.vsd.hh.ui.ucSearchOperatorProfile   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6
propertiesclr-namespace:VSDApp.Properties     5      \
extToolkitEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.Extended     5      -   5        $
��Auto��5	      $
��Auto��6   
�������  � 	Resources  5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      .  (Microsoft.Windows.Controls.BusyIndicator  -   5          Name$ busyIndicator��5         IsBusy .  .��5      �  � RowDefinitions 5      �� 5      $��* �6$   �� 5      $
��Auto �6$   �� 5      $��5 �6$   �� 5      $
��Auto �6$   �� 5      $��5 �6$   �� 5      $
��Auto �6$   �� 5      $��5 �6$   �� 5       $
��Auto �6$   �� 5!      $��5 �6$   �� 5"      $
��Auto �6$   �� 5#      $��5 �6$   �� 5$      $
��Auto �6$   �� 5%      $��5 �6$   �� 5&      $
��Auto �6$   �� 5'      $��5 �6$   �� 5(      $
��Auto �6$   �� 5)      $��5 �6$   �� 5+      $��* �6$    � ColumnDefinitions 5.      �� 5/      $��* �6'   �� 50      $
��Auto �6'   �� 51      $��5 �6'   �� 52      $
��Auto �6'   �� 53      $��15 �6'   �� 54      $
��Auto �6'   �� 55      $��15 �6'   �� 56      $
��Auto �6'   �� 58      $��5 �6'   �� 59      $��25 �6'   �� 5:      $��* �6'   .+�5=      �� -   5         ��Name$ lblOprProfile��5=      !   VSDApp.Properties.Resources   MenueOperatorProfile#��Z 6+   $��Segoe UI)�6k   $��Medium"�6�   $��30(�6�   $��1��6�   $��1��6�   $��Stretch=�6�   $��Stretch=�6�   �� 5A      -   5        $ lblOpetatoName2��5A      	   OperatorName2#��Z	 6.   $��Bottom=�6f   $
��Left=�6�   $��3��6�   $��1��6�   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5B      -   5        
 ��Name$
 txtOperatorName��5B      $��3��6/   $��3��6<      TextBoxLarge#��[  6L   $
��Left=�6r   $��Stretch=�6�   6R  �� 5C      -   5        $ lblTraficeFileNo��5C         TrafficeFileNumber#��Z 6.   $
��Left=�6k   $��5��6�   $��1��6�   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5E      -   5        $
 txtTraffice��5E      $��5��6+   $��3��68    � 
InputScope$ Number��6H   #��[  6\   $��Bottom=�6�   $
��Left=�6�   6]  �� 5G      -   5         ��Name$ btnStackePanel��5G      $��7��65   $��3��6B    �� Orientation$ 
Horizontal=�6R   $��Center=�6k   $��Right=�5H      .+�5I      �  5K      �� 5L      $��* �6(    5O      �� 5P      $��* �6+   �� 5Q      $��* �6+   .+�5S      �� -	   5         ��Name$ btnResetVehicleRecord��5S         BtnReset#��Z 6>   $��0,0,0,0q�6q   $��0��6�   $��0��6�     ButtonBackHalfControl#��[ 6�   6�   �� 5T      -
   5        $ 	btnSearch��5T         	BtnSearch#��Z 62   $��10,0,0,0q�6f     ButtontoProceedHalfControl#��[ 6x   $��0��6�   $��1��6�   6�   �� 5V      -   5         ��Name$  imagebtnResetVehicleRecord��5V      $y�	Collapsed=�6@   $'��!..\Images\Buttons\Small\Reset.png��6W   $	��150��6�   $��7��6�   $��7��6�   $��Stretch=�6�   $��Stretch=�6�   61  �� 5W      -   5        $ imagebtnSearch��5W      $y�	Collapsed=�64   $(��"..\Images\Buttons\Small\Search.png��6K   $	��150��6w   $��7��6�   $��7��6�   $��30 0 0 0q�6�   $��Stretch=�6�   $��Stretch=�6�   6+  5X      5Z      �� 5[      -   5        $ lblOpPrfile��5[      $y�	Collapsed=�6)      OperatorProfile#��Z 6@   $��Segoe UI)�6z   $��Medium"�6�   $��30(�6�   $��9��6�   $��1��6�   $��3��6�   $��Stretch=�6�   $��Stretch=�6  �� 5]      -   5        $ stkPnleOVRR��5]      $��11��6.   $��1��6<   $ 
Horizontal=�6L   .+�5_      �� -   5        $ 
lblOprOVRR��5_         OVRR#��Z 6,   $
��Left=�6[   $��Segoe UI)�6w   $��Medium"�6�   $��25(�6�   �� 5`      -   5        $
 txtOVVRScore��5`      $y�	Collapsed=�6,   $��11��6C   $��3��6Q    �� 
IsReadOnly . #��[  6s   $
��Left=�6�   $��Bottom=�6�   $��20(�6  $��Segoe UI)�6-  $��Medium"�6C  
�������6j  � 5a      -   5         �Name$ grdcanvasVRR��5a      $��50��6*   $��40��65   $��50 0 0 0q�6A   $��Right=�6S   .+�5b      �� -   5         ��Name$ canvVRR��5b      �� 5c      -   5        $  lblRattingTextwithColorVRR��5c      $��Normal"�6>   $��Center=�6S   
�������$��Center=�6�   $��Center=�6�   $��18(�6�   5d      5e      �� 5f      -   5        $ stkPnleODRR��5f      $��11��6.   $��3��6<   $ 
Horizontal=�6L   .+�5g      �� -   5        $ 
lblOprODRR��5g         ODRR#��Z 6,   $
��Left=�6[   $��Segoe UI)�6w   $��Medium"�6�   $��25(�6�   � 5h      -   5        $ grdcanvasDRR��5h      $��50��6.   $��40��69   $��50 0 0 0q�6E   $��Right=�6W   .+�5i      �� -   5        $ canvDRR��5i      �� 5j      -   5        $  lblRattingTextwithColorDRR��5j      $��Normal"�6>   $��Center=�6S   
�������$��Center=�6�   $��Center=�6�   $��18(�6�   5k      5l      �� 5n      -   5        $ lblTopFiveViolation��5n         TopFiveViolatingVehicles#��Z 61   $
��Left=�6t   $��13��6�   $��1��6�   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   &   System.Windows.Controls.DataGrid  5o      -   5          Name$ grdOperatorProfile��5o      $��13��63   $��3��6A   $	��320��6Q   $	��100��6]      AutoGenerateColumns .  $
��Left=�6�      CanUserAddRows .     
IsReadOnly .  $��15(�5p   �   $��Segoe UI)�6�   $��Medium"�6�   ��6   �� *operatorData+ �� UpdateSourceTrigger$ PropertyChanged=�   Columns 5q      0  *System.Windows.Controls.DataGridTextColumn  5r          PlateDetails,  &System.Windows.Controls.DataGridColumn!   Header#! Z  6)   "   
IsReadOnly" .  #   Width5  /System.Windows.Controls.DataGridLengthConverter$# * 6s   $   FontSize$$ 15Q�6�   %   
FontFamily$% Segoe UI)�6�   &   
FontWeight$& Medium"�6�   1  +System.Windows.Controls.DataGridBoundColumn'   Binding' 6}   �� *PlateDetails+6�     5s      (   
RiskRating#! Z( 6)   " .  $# * 6q   $$ 15Q�6�   $% Segoe UI)�6�   $& Medium"�6�   ' 6{   �� *
RiskRating+6�   4  .System.Windows.Controls.DataGridTemplateColumn  5t      )   Inspect2#! Z) 6-   " . *   
Visibility$* Visible=�6q   +   CellTemplate+ 5u      �� 5v      .R�5w   "   �� $ btnStartInspection��6)   -   5        #��Z) 5w   E   $y�Visible=�6x   $��Delete��6�   $��15(�6�   5x      5z      5      �� 5�      -   5        $ btnBack��5�      $��15��6&   $��3��64   ,   BtnBack#��Z, 6D   #��[ 6v   $��0,0,0,0q�6�   $��Right=�6�   6�   �� 5�      -   5        $ imagebtnBack��5�      $y�	Collapsed=�6*   $&�� ..\Images\Buttons\Small\Back.png��6A   $	��150��6k   $��15��6w   $��3��6�   $��Right=�6�   $��Stretch=�6�   6  5�      5�      5�      