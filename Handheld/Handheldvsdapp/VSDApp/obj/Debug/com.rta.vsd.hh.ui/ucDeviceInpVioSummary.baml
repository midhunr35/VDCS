   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      4    .VSDApp.com.rta.vsd.hh.ui.ucDeviceInpVioSummary   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6
propertiesclr-namespace:VSDApp.Properties     5      -   5        $
��Auto��5      $
��Auto��6   $��Stretch=�6)   $��Stretch=�6G   $��Stretch=�5      $��Stretch=�6*   
�������  � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      �� $
��Auto=�6   $
��Auto=�66   .��5   
   �  � RowDefinitions 5      �� 5      $��1 �6    �� 5      $��* �6    �� 5      $
��Auto �6    �� 5      $��5 �6    �� 5      $
��Auto �6    �� 5      $��5 �6    �� 5      $
��Auto �6    �� 5      $��5 �6    �� 5      $
��Auto �6    �� 5       $��5 �6    �� 5!      $
��Auto �6    �� 5"      $��5 �6    �� 5#      $
��Auto �6    �� 5$      $��5 �6    �� 5%      $
��Auto �6    �� 5&      $��5 �6    �� 5'      $
��Auto �6    �� 5(      $��5 �6    �� 5)      $
��Auto �6    �� 5*      $��5 �6    �� 5+      $
��Auto �6    �� 5,      $��* �6    �� 5-      $��* �6     � ColumnDefinitions 5/      �� 50      $��* �6#   �� 51      $
��Auto �6#   �� 52      $��10 �6#   �� 53      $
��Auto �6#   �� 54      $��* �6#   .+�56      �� -   5         ��Name$ 
lblViolSum��56      !   VSDApp.Properties.Resources   ViolationSummary#��Z 6(   $��Segoe UI)�6c   $��Medium"�6y   $��30(�6�   $��2��6�   $��3��6�   $��1��6�   $
��Left=�6�   $��Stretch=�6�   �� 57      -   5        $ lblOvrAllSev��57         OverAllViolationSeverity#��Z 6*   $��4��6m   $��1��6z   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 58      -   5         ��Name$ txtboxViolationSeverity��58      $��4��67   $��3��6D   $	��350��6T   $��40��6`   $��20(�6l   $��Segoe UI)�6z   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 �� 
IsReadOnly	 . 6�   �� 59      -   5        $ 	lblVioSum��59      
   ViolationID#��Z
 6'   $��6��6]   $��1��6j   $��Segoe UI)�6z   $��Medium"�6�   $��25(�6�   �� 5:      -   5        $ txtViolationID��5:      $��6��6.   $��3��6;   $	��350��6K   $��40��6W   $��20(�6c   $��Segoe UI)�6q   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 . 6�   �� 5;      -   5        $ 
lblDueDate��5;         DueDate#��Z 6(   $��8��6Z   $��1��6g   $��Segoe UI)�6w   $��Medium"�6�   $��25(�6�   �� 5<      -   5        $ txtboxDueDate��5<      $��8��6-   $��3��6:   $	��350��6J   $��40��6V   $��20(�6b   $��Segoe UI)�6p   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 . 6�   �� 5?      -	   5         ��Name$ btnStackePanel��5?      $��20��61   $��3��6?    �� Orientation$ 
Horizontal=�6O   $��Center=�6h   $��Right=�5@      .+�5A      �  5C      �� 5D      $��* �6(    5G      �� 5H      $��* �6+   �� 5I      $��* �6+   .+�5K      �� -
   5         ��Name$ btnPrint��5K   �      BtnPrint#��Z 6   $��0��6P   $��1��6]   $y�Visible=�6m      ButtontoProceedHalfControl#��[  6�   $
��Left=�6�   $��20,0,0,0q�6�   6�   �� 5L      -   5        $ btnback��5L   �      BtnBack#��Z 6   $��0��6O   $��0��6\   $y�Visible=�6l     ButtonBackHalfControl#��[ 6�   $
��Left=�6�   $��0,0,0,0q�6�   6�   5M       5Q      5R      5T      5U      