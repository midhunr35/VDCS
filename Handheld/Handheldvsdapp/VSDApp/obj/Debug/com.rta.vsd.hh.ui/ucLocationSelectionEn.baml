   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      4    .VSDApp.com.rta.vsd.hh.ui.ucLocationSelectionEn   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6
propertiesclr-namespace:VSDApp.Properties     5      -   5        $
��Auto��5      $
��Auto��6   $��Stretch=�6)   $��Stretch=�6G   
�������$��Stretch=�5      $��Stretch=�6*     � 	Resources  5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��10 �6   �� 5      $
��Auto �6   �� 5      $��20 �6   �� 5      $
��Auto �6   �� 5      $��20 �6   �� 5      $
��Auto �6   �� 5       $��20 �6   �� 5!      $
��Auto �6   �� 5"      $��20 �6   �� 5#      $��* �6   �� 5$      $��* �6    � ColumnDefinitions 5&   
   �� 5'      $��* �6   �� 5(      $
��Auto �6   �� 5)      $��5 �6   �� 5*      $
��Auto �6   �� 5+      $��5 �6   �� 5-      $��* �6   .+�50   
   �� -   5         ��Name$ lblLocationDetail��50      !   VSDApp.Properties.Resources   LocationDetails#��Z 6+   $��Segoe UI)�6e   $��Medium"�6{   $��30(�6�   $��1��6�   $��1��6�   $��Stretch=�6�   $��Stretch=�6�   �� 51   
   -   5        $ lblWouldYouLike��51         lblWouldYouLike#��Z 6)   $��15(�6e   $y�	Collapsed=�6s   �� 52   
   -   5        $ lblAppLogout��52         lblAppLogout#��Z 6&   $��15(�6_   $y�	Collapsed=�6m   �� 53   
   -   5        $ 
lblEmirate��53      	   Emirates#��Z	 6$   $��3��6X   $��1��6e   $��Segoe UI)�6u   $��Medium"�6�   $��25(�6�   �� 54   
   -   5        
 ��Name$
 cmboxEmirates��54         cmbLargeSize#��[  6*   $��3��6P   $��3��6]   $
��Left=�6o   $��Center=�6�   $��Center=�6�   6�  �� 55   
   -   5        $ lblArea��55         Area#��Z 6!   $��5��6Q   $��1��6^   $��Segoe UI)�6n   $��Medium"�6�   $��25(�6�   �� 56   
   -   5        $
 	cmboxArea��56      $��5��6&   $��3��63   #��[  6D   $
��Left=�6j   $��Center=�6�   $��Center=�6�   6�  �� 57   
   -	   5        $ lblLoc��57         Location#��Z 6    $��7��6S   $��1��6`   $��Segoe UI)�6p   $��Medium"�6�   $��25(�6�   �� 58   
   -
   5        $
 cmboxLocation��58      $��7��6*   $��3��67   #��[  6G   $
��Left=�6m    �� 
IsEditable .  �� IsTextSearchEnabled . $��Center=�6�   $��Center=�6�   6�  �� 59   
   -   5         ��Name$ ContentControlShowMap��59      $y�	Collapsed=�68   $��1��6O   $��5��6\   $��2��6l   $
��Auto��6�   $��11��6�   �� 5:   
   -   5         ��Name$ imag_location��5:      $��1��6'   $y�	Collapsed=�64   $��5��6K   $��2��6[   $
��Auto��6p   $��11��6}   �� 5<   
   -   5        $��
tbLocation��5<   k   $��5��6   $��16(�6"   
�������$��29��6C   $
��Left=�6O   $��25.520581,-103.40607��6}   $y�	Collapsed=�6�   $	��Top=�6�   $	��168��6�   $��0q�6�   .��6�   ��5=      �� 5>      .��5?   "   5@      �� 5C   
   -   5         ��Name$ btnStackePanel��5C      $��9��6-   $��3��6;    �� Orientation$ 
Horizontal=�6K   $��Center=�6d   .+�5D      �  5F      �� 5G      $��* �6$    5J      �� 5K      $��* �6'   �� 5L      $��* �6'   .+�5N      �� -   5         ��Name$ btnback��5N         BtnBack#��Z 6*     ButtonBackHalfControl#��[ 6\   $��0 0 0 0q�6�   $��0��6�   $��0��6�   $
��Left=�6�   6�   �� 5O      -   5        $ btnNext��5O         Inspect#��Z 6+     ButtontoProceedHalfControl#��[ 6a   $��0��6�   $��1��6�   $��20(�6�   $��8 0 0 0q�6�   $��Right=�6�   �� 5P      -   5        $ imagebtnback��5P      $y�	Collapsed=�6.   $&�� ..\Images\Buttons\Small\Back.png��6F   $	��140��6p   $��Stretch=�6}   $��Stretch=�6�   6�   �� 5Q      -   5        $ imagebtnNext��5Q      $y�	Collapsed=�6.   $2��,..\Images\Buttons\Small\Start Inspection.png��6E   $	��140��6{   $��Stretch=�6�   $��Stretch=�6�   6   5S      5T      5U      5W      