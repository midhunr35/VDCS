   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      Z VWPFToolkit.Extended, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4dEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.ExtendedMicrosoft.Windows.Controls 5      .    (VSDApp.com.rta.vsd.hh.ui.ucGoodByeScreen   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6
propertiesclr-namespace:VSDApp.Properties     5      \
extToolkitEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.Extended     5      -   5        $
��Auto��5      $
��Auto��6   $��Stretch=�6(   $��Stretch=�6F   $��Stretch=�5	      $��Stretch=�63     � 	Resources  5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      � -   5         �Name$ GridMain��5      $y�Visible=�6   ���#F1F1F1 � RowDefinitions 5      �� 5      $��* �6    �� 5      $
��0.5* �6    �� 5      $��0 �6   �� 5      $
��Auto �6   �� 5      $��2 �6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6   .+�5       �� -   5         ��Name$ stckPanelUpperMain��5        �� Orientation$ 
Horizontal=�65   $��0��6N   $��0��6[   $��Center=�6k   .+�5!      �� -   5        $ stackPanelUpperLeft��5!      $��Center=�6:   .+�5"      �  5#      �� 5$      $��5 �6,   �� 5%      $
��Auto �6,   �� 5&      $��3 �6,   �� 5'      $
��Auto �6,   �� 5(      $��3 �6,   �� 5)      $
��Auto �6,   �� 5*      $��3 �6,   �� 5+      $
��Auto �6,   �� 5,      $��3 �6,   �� 5-      $
��Auto �6,   �� 5.      $��0 �6,    51      �� 52      $��15 �6/   �� 53      $��* �6/   �� 54      $��15 �6/   .+�57      �� !   VSDApp.Properties.Resources   
lblGoodBye#��Z 6!   $
��Left=�6V   $
��Left=�6r   $��1��6�   $��1��6�   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 58      -   5        	 ��Name$	 lblInspectorName��58       $y�	Collapsed=�6:   $��Center=�6Q   $��Center=�6o   $��3��6�   $��1��6�   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5:      $ 
Horizontal=�6)   $��3��6B   $��1��6O   $y�	Collapsed=�6_   .+�5;   "   �� 
   lblTotalVehicleInspected#��Z
 6)   $��Center=�6l   $��Center=�6�   $��Segoe UI)�6�   $��20(�6�   $��Medium"�6�   �� 5<   "   -   5         ��Name$ lblTotalVehicleInspected��5<   *   $��60��6M   $��Center=�6Y   $��Center=�6v   $��Center=�6�   $��Center=�6�   $��Segoe UI)�6�   $��25(�6�   5=      �� 5>      -   5        $ StackPanelResult��5>   !   $ Vertical=�6;   $��3��6R   $��1��6_   .+�5?      �� -   5        $ spInspections��5?   %   $ 
Horizontal=�6<   .+�5@      ��    TotalInspection#��Z 6%   $
��Left=�6_   $
��Left=�6{   $��0 0 0 0q�6�   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   �� 5A      -	   5        $	 lblTotalInspection��5A   $   $��0��6@   
���  ��$
��Left=�6^   $
��Left=�6y   $��0 0 0 0q�6�   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   6�   5B      �� 5C      -
   5        $ spInspectionWithFine��5C   %   $ 
Horizontal=�6C   .+�5D      ��    InspectionWithFine#��Z 6%   $
��Left=�6b   $
��Left=�6~   $��0 0 0 0q�6�   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   �� 5E      -   5        $	 lblInspectionWithFine��5E   $   $��0��6C   
���  ��$
��Left=�6a   $
��Left=�6|   $��0 0 0 0q�6�   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   6�   5F      �� 5G      -   5        $ spInspectionWithoutFine��5G   %   $ 
Horizontal=�6F   .+�5H      ��    InspectionWithoutFine#��Z 6%   $
��Left=�6e   $
��Left=�6�   $��0 0 0 0q�6�   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   �� 5I      -   5        $	 lblInspectionWithoutFine��5I   $   $��0��6F   
���  ��$
��Left=�6d   $
��Left=�6   $��0 0 0 0q�6�   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   6  5J      �� 5K      -   5        $ spTotalViolations��5K   %   $ 
Horizontal=�6@   .+�5L      ��    TotalViolations#��Z 6%   $
��Left=�6_   $
��Left=�6z   $��0 0 0 0q�6�   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   �� 5M      -   5        $	 lblTotalViolations��5M   $   $��0��6@   
���  ��$
��Left=�6^   $
��Left=�6y   $��0 0 0 0q�6�   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   6  5N      �� 5O      -   5        $ spFineAmount��5O   %   $ 
Horizontal=�6;   .+�5P      ��    
FineAmount#��Z 6%   $
��Left=�6Z   $
��Left=�6u   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   �� 5Q      -   5        $	 lblFineAmount��5Q   $   $��0��6;   
���  ��$��Center=�6Y   $
��Left=�6v   $��Segoe UI)�6�   $��Medium"�6�   $��20(�6�   6  5R      �� 5T      -   5        $ 	spMessage��5T   %   $ 
Horizontal=�68   $��0 0 0 0q�6Q   .+�5U      �� -   5         ��Name$ txtblkMessage��5U   (      goodByScreenMsg#��Z 6?   $
��Wrap=�5V       $
��Left=�65   $��0 0 0 0q�6Q   $	��330��5W      $��Segoe UI)�6+   $��Normal"�6A   $��16(�6V   
���  ��6d   5X      5Y      �� 5[      -   5        $	 lblAlwaysReme��5[         GoodByeQuote1#��Z 63   $��5��6k   $��1��6x   $
��Left=�6�   $��Center=�6�   $��Segoe UI)�6�   $��15(�6�   �� 5\      -   5        $	 lblCreativity��5\         GoodByeQuote2#��Z 63   $��7��6k   $��1��6x   $
��Left=�6�   $
��Left=�6�   $y�	Collapsed=�6�   $��Segoe UI)�6�   $��15(�6�   �� 5]      -   5        $	 
lblQuotes3��5]         GoodByeQuote3#��Z 60   $��7��6h   $��1��6u   $
��Left=�6�   $
��Left=�6�   $��Segoe UI)�6�   $��15(�6�   5_      5`      �� 5a      -   5        $ stackPanelUpperRight��5a      .+�5b      �  5c      �� 5d      $��* �6(   �� 5e      $
��0.5* �6(    5g      �� 5h      $��* �6+   .+�5j      �� -   5         ��Name$ imageRTAInsp��5j      $��0��62   $��0��6?   $*��$..\Images\Inspectors\hialbedwawi.png��6O   $	��200��6}   $��0,20,0,0q�6�   5n      5p      5r      �� 5s      -   5        $ stckPanelUpperMain2��5s      $��1��67   $��3��6D   .+�5t      �� $#��..\Images\login_landscape.jpg��6   5u      � 5v      -   5        $ maingrid��5v      $y�	Collapsed=�6%    5x      �� 5y      $��* �6$   �� 5z      $
��Auto �6$   �� 5{      $��10 �6$   �� 5|      $
��Auto �6$   �� 5}      $��10 �6$   �� 5~      $
��Auto �6$   �� 5      $��10 �6$   �� 5�      $
��Auto �6$   �� 5�      $��10 �6$   �� 5�      $
��Auto �6$   �� 5�      $��10 �6$   �� 5�      $
��Auto �6$   �� 5�      $��10 �6$   �� 5�      $
��Auto �6$   �� 5�      $��* �6$    5�      �� 5�      $��* �6'   �� 5�      $
��Auto �6'   �� 5�      $��10 �6'   �� 5�      $
��Auto �6'   �� 5�      $��10 �6'   �� 5�      $
��Auto �6'   �� 5�      $��10 �6'   �� 5�      $
��Auto �6'   �� 5�      $��* �6'   .+�5�      �� #��Z 6   $��Center=�6N   $��Center=�6l   $��1��6�   $��3��6�   $��Segoe UI)�6�   $��Medium"�6�   $��30(�6�   �� 5�      -   5        $	 lblInspectionResult��5�         lblInspectionResult#��Z 65   $��Center=�6s   $��Center=�6�   $��3��6�   $��5��6�   $��Segoe UI)�6�   $��Medium"�6�   $��30(�6�   �� 5�      -   5        $ imagebtnNext��5�      $��9��6.   $��7��6;   $y�	Collapsed=�6K   $&�� ..\Images\Buttons\Small\Next.png��6b   $	��140��6�   $��30 0 0 0q�6�   $��Right=�6�   $��Stretch=�6�   6  5�      �� 5�   
   -   5         ��Name$ btnBackImage3��5�      $��0��6(   $��1��65   $��Bottom=�6E      BtnBack#��Z 6a   $��10,0,0,0q�6�   $��35��6�   $	��140��6�      ButtonBackHalfControl#��[  6�   $��Right=�6�   6  5�      5�      