   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      Z VWPFToolkit.Extended, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4dEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.ExtendedMicrosoft.Windows.Controls 5      .    (VSDApp.com.rta.vsd.hh.ui.ucSearchVehicle   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      6
propertiesclr-namespace:VSDApp.Properties     5      \
extToolkitEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.Extended     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        
�������$
��Auto��5   K   $
��Auto��6Y     � 	Resources  5	       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6$   �� 5      $
��Auto �6$   �� 5      $��15 �6$   �� 5      $
��Auto �6$   �� 5      $��15 �6$   �� 5      $
��Auto �6$   �� 5      $��15 �6$   �� 5      $
��Auto �6$   �� 5      $��15 �6$   �� 5      $
��Auto �6$   �� 5      $��15 �6$   �� 5       $
��Auto �6$   �� 5!      $��15 �6$   �� 5"      $
��Auto �6$   �� 5#      $��30 �6$   �� 5$      $��* �6$   �� 5%      $��* �6    � ColumnDefinitions 5(      �� 5)      $��* �6'   �� 5*      $
��Auto �6'   �� 5+      $��5 �6'   �� 5,      $
��Auto �6'   �� 5-      $��5 �6'   �� 5.      $��* �6'   .+�50   
   �� -   5         ��Name$ lblVehProfile��50      !   VSDApp.Properties.Resources   MenueVehicleProfile#��Z 6(   $��Segoe UI)�6f   $��Medium"�6|   $��30(�6�   $��1��6�   $��1��6�   $��Stretch=�6�   $��Stretch=�6�   �� 51   
   -   5        $ 
lblCountry��51      $
��Left=�6%   $��3��6@   $��1��6M      Country#��Z 6]   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 52   
   -   5         ��Name$ cmboxCountry��52      $��3��6)   $��3��66      cmbLargeSize#��[  6F   	 �� 
IsEditable	 . 
 �� IsTextSearchEnabled
 . $
��Left=�6�   $��Bottom=�6�   6�  �� 53   
   -   5        $ 
lblEmirate��53      $
��Left=�6%   $��5��6@   $��1��6M      Emirates#��Z 6]   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 54   
   -   5        $ cmboxEmirates��54      $��5��6*   $��3��67   	 . 
 . #��[  6t   $
��Left=�6�   $��Bottom=�6�   6�  �� 55   
   -   5        $ lblPlateCateogry��55      $
��Left=�6*   $��7��6E   $��1��6R      PlateCatogory#��Z 6b   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 56   
   -   5        $ cmboxPlateCategory��56      $��7��6/   $��3��6<   	 . 
 . #��[  6y   $
��Left=�6�   $��Bottom=�6�   6�  �� 57   
   -	   5        $ 
lblPlateNo��57      $
��Left=�6%   $��9��6@   $��1��6M      PlateNumber#��Z 6]   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 58   
   -
   5         ��Name$ txtBoxPlateNumber��58      $��9��6-    � 
InputScope$ Number��6:   $��3��6N     TextBoxLarge#��[ 6^   $
��Left=�6�   $��Bottom=�6�   6k  �� 59   
   -   5        $ lblPlateCode��59      $
��Left=�6'   $��11��6B   $��1��6P      	PlateCode#��Z 6`   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5:   
   -   5        $ cmboxPlateCode��5:      $��11��6+   $��3��69   	 . 
 . #��[  6v   $
��Left=�6�   $��Bottom=�6�   6�  �� 5;      -   5        $ lblSearchingVehicle��5;         lblSearchingVehicle#��Z 61   $��20(�6p   $��Segoe UI)�6~   $��Medium"�6�   $��0��6�   $��3��6�   $��Stretch=�6�   $��Stretch=�6�   $y�Hidden=�6�   �� 5=      -   5         ��Name$ btnStackePanel��5=      $��13��61   $��3��6?    �� Orientation$ 
Horizontal=�6O   $��Center=�6h   $
��Left=�5>      .+�5?      �  5A      �� 5B      $��* �6$    5E      �� 5F      $��* �6'   �� 5G      $��* �6'   .+�5I      �� -   5         ��Name$ btnResetVehicleRecord��5I      $y�Visible=�68      BtnBack#��Z 6N   $��0,0,0,0q�6�     ButtonBackHalfControl#��[ 6�   $��0��6�   $��0��6�   6�   �� 5J      -   5        $ 	btnSearch��5J      $y�Visible=�6,   $��10,0,0,0q�6B      	BtnSearch#��Z 6U     ButtontoProceedHalfControl#��[ 6�   $��0��6�   $��1��6�   6�   �� 5M      -   5         ��Name$ imagebtnBack��5M      $y�	Collapsed=�6*   $&�� ..\Images\Buttons\Small\Back.png��6A   $	��150��6k   $��15��6w   $��3��6�   $��Right=�6�   $��Stretch=�6�   6  �� 5N      -   5        $ imagebtnSearch��5N      $y�	Collapsed=�6,   $(��"..\Images\Buttons\Small\Search.png��6C   $	��150��6o   $��7��6{   $��7��6�   $��40 0 0 0q�6�   $��Stretch=�6�   $��Stretch=�6�   6#  5P      5Q      5R      5V      