   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      =    7VSDApp.com.rta.vsd.hh.ui.ucRecordViolationNonUAEVehicle   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6
propertiesclr-namespace:VSDApp.Properties     5      -   5        $
��Auto��5      $
��Auto��6-     � 	Resources  5	       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      � -   5         �Name$ grdVehSerch��5      $y�Visible=�6     � RowDefinitions 5   
   �� 5      $��5 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5       $��5 �6    � ColumnDefinitions 5&   
   �� 5'      $��* �6   �� 5(      $
��Auto �6   �� 5)      $��2 �6   �� 5*      $
��Auto �6   �� 5+      $��2 �6   �� 5,      $
��Auto �6   �� 5-      $��2 �6   �� 5.      $
��Auto �6   �� 5/      $��* �6   .+�51   
   �� -   5         ��Name$ 
lblOprName��51      $
��Left=�6$   $��1��6?   $��1��6L   !   VSDApp.Properties.Resources   OperatorName#��Z 6\   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 52   
   -   5         ��Name$ txtBoxOperatorName��52      $��1��6.   $��3��6;   $��Bottom=�6K      TextBoxLarge#��[  6f   $
��Left=�6�   6\  �� 53   
   -   5        $ lblChassisNo��53      $
��Left=�6&   $��1��6A   $��5��6N   	   ChassisNumber#��Z	 6^   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 54   
   -   5        $ txtBoxChassisNumber��54   '   
 � 
InputScope$
 Number��6   $��1��6D   $��7��6Q   #��[  6a   $
��Left=�6�   $��Bottom=�6�   6B  �� 55   
   -   5        $ lblMake��55      $
��Left=�6!   $��3��6<   $��1��6I      Make#��Z 6Y   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 56   
   -   5        $ 
txtBoxMake��56      $��3��6&   $��3��63   #��[  6C   $
��Left=�6i   $��Bottom=�6�   6$  �� 57   
   -	   5        $ lblModel��57      $
��Left=�6"   $��3��6=   $��5��6J      Model#��Z 6Z   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 58   
   -
   5        $ txtModel��58      $��3��6$   $��7��61   #��[  6A   $
��Left=�6g   $��Bottom=�6�   6"  �� 59   
   -   5        $ lblYear��59      $
��Left=�6!   $��5��6<   $��1��6I      Year#��Z 6Y   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 5:   
   -   5        $ txtYear��5:      $��5��6#   $��3��60   #��[  6@   $
��Left=�6f   $��Bottom=�6�   6E  �� 5;   
   -   5         ��Name$ btnStackePanel��5;      $��5��6-   $��7��6:    �� Orientation$ 
Horizontal=�6J   $��Center=�6c   $��Right=�5<      .+�5=      �  5?      �� 5@      $��* �6$    5C      �� 5D      $��* �6'   �� 5E      $��* �6'   .+�5G      �� -   5         ��Name$ btnBack��5G      $��0��6*   $��0��67      BtnBack#��Z 6H     ButtonBackHalfControl#��[ 6z   $��0,0,0,0q�6�   $
��Left=�6�   �� 5H      -   5        $ btnStartInspection��5H      $��0��65   $��1��6B      Inspect#��Z 6R     ButtontoProceedHalfControl#��[ 6�   $��10,0,0,0q�6�   $
��Left=�6�   6�   �� 5I      -   5         ��Name$ imagebtnback��5I      $y�	Collapsed=�6.   $&�� ..\Images\Buttons\Small\Back.png��6E   $	��140��6o   $��0 0 0 0q�6{   $��Stretch=�6�   $��Stretch=�6�   6  �� 5J      -   5        $ imagebtnNext��5J      $y�	Collapsed=�6.   $2��,..\Images\Buttons\Small\Start Inspection.png��6E   $	��140��6{   $��30 0 0 0q�6�   $��Stretch=�6�   $��Stretch=�6�   6  5K      5M      5N      5P      