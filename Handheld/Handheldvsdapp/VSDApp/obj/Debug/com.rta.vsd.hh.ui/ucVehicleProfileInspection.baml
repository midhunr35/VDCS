   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      9    3VSDApp.com.rta.vsd.hh.ui.ucVehicleProfileInspection   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      6
propertiesclr-namespace:VSDApp.Properties     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        $
��Auto��5      $
��Auto��6     � 	Resources  5	       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      � -   5         �Name$ grdVehSerch��5      $y�Visible=�6     � RowDefinitions 5   
   �� 5      $��0 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5      $��7 �6   �� 5      $
��Auto �6   �� 5       $��7 �6   �� 5!      $
��Auto �6   �� 5"      $��7 �6   �� 5#      $
��Auto �6   �� 5$      $��2 �6    � ColumnDefinitions 5(   
   �� 5)      $��* �6   �� 5*      $
��Auto �6   �� 5+      $��2 �6   �� 5,      $
��Auto �6   �� 5-      $��2 �6   �� 5.      $
��Auto �6   �� 5/      $��2 �6   �� 50      $
��Auto �6   �� 51      $��* �6   .+�53   
   �� -   5         ��Name$ 
lblOprName��53      $
��Left=�6$   $��1��6?   $��1��6L   !   VSDApp.Properties.Resources   OperatorName#��Z 6\   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 54   
   -   5         ��Name$ txtBoxOperatorName��54      $��1��6.   $��3��6;      TextBoxLarge#��[  6K   	 �� 
IsReadOnly	 .  68  �� 55   
   -   5        $ lblChassisNo��55      $
��Left=�6&   $��1��6A   $��5��6N   
   ChassisNumber#��Z
 6^   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 56   
   -   5        $ txtBoxChassisNumber��56      $��1��6/   $��7��6<   #��[  6L   	 .  6
  �� 57   
   -   5        $ lblMake��57      $
��Left=�6!   $��3��6<   $��1��6I      Make#��Z 6Y   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 58   
   -   5        $ 
txtBoxMake��58      $��3��6&   $��3��63   #��[  6C   	 .  6  �� 59   
   -	   5        $ lblModel��59      $
��Left=�6"   $��3��6=   $��5��6J      Model#��Z 6Z   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 5:   
   -
   5        $ txtModel��5:      $��3��6$   $��7��61   #��[  6A   	 .  6�   �� 5<   
   -   5        $ lblYear��5<      $
��Left=�6!   $��7��6<   $��1��6I      Year#��Z 6Y   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 5=   
   -   5        $ txtYear��5=      $��7��6#   $��3��60   #��[  6@   $��Bottom=�6f   6  � 5?   
   -   5        $ 	grdcanvas��5?      $��7��6"   $��7��6/   $��50��6?   $��40��6J   .+�5@      �� -   5         ��Name$
 canv��5@      �� 5A      -   5        $ lblRattingTextwithColor��5A      $��G0��65   $��3��6B   $��7��6O   $
��Bold"�6_   $��Center=�6r   
�������$��Center=�6�   $��Center=�6�   $��20(�6�   5B      �� 5D   
   -   5        $ lblRecomendation��5D      $
��Left=�6*   $��9��6E   $��1��6R      Recomendations#��Z 6b   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   �� 5E   
   -   5        $ txtRecomendation��5E      	 . $��9��6>   $��3��6K   #��[  6[   $��Bottom=�6�   6!  �� 5F   
   -   5        $ lblVRR��5F      $
��Left=�6    $��7��6;   $��5��6H      VRRScore#��Z 6X   $��Arial Rounded MT Bold)�6�   $
��Bold"�6�   $��25(�6�   &   System.Windows.Controls.DataGrid  5H      -   5          Name$ grdOpenViolations��5H      $��9��61   $��3��6>   $	��300��6N   $��50��6Z      AutoGenerateColumns .  $y�	Collapsed=�6�      CanUserAddRows .  ��5I      �� *openViolations+ �� UpdateSourceTrigger$ PropertyChanged=�   Columns 5K      0  *System.Windows.Controls.DataGridTextColumn  5L      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6C   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6M   �� *ViolationTicketCode+6w     5M      $ 
Issue Date��6%    . $ * 6K    6U   �� *	IssueDate+6u     5N      $ Status��6%    . $ * 6G    6Q   �� *ViolationStatus+6w     5O      $ ViolationSeverity��6%    . $ * 6R    6\   �� *ViolationSeverity+6�   5S      �� 5T   
   -   5         ��Name$ btnStackePanel��5T      $��9��6-   $��7��6:    �� Orientation$ 
Horizontal=�6J   $��Center=�6c   $��Right=�5U      .+�5V      �  5X      �� 5Y      $��* �6$    5\      �� 5]      $��* �6'   �� 5^      $��* �6'   .+�5a      �� -   5         ��Name$ btnBack��5a      $��0��6*   $��0��67      BtnBack#��Z 6H   $��0,0,0,0q�6z   $
��Left=�6�     ButtonBackHalfControl#��[ 6�   6�   �� 5b      -   5        $ btnStartInspection��5b      $��0��65   $��1��6B      Inspect#��Z 6S   $��10,0,0,0q�6�     ButtontoProceedHalfControl#��[ 6�   $
��Left=�6�   6�   �� 5c      -   5         ��Name$ imagebtnback��5c      $y�	Collapsed=�6.   $&�� ..\Images\Buttons\Small\Back.png��6E   $��Stretch=�6o   $��Stretch=�6�   6�   �� 5d      -   5        $ imagebtnNext��5d      $y�	Collapsed=�6.   $2��,..\Images\Buttons\Small\Start Inspection.png��6E   $��Stretch=�6{   $��Stretch=�6�   6�   5e      5f      5g      5h      