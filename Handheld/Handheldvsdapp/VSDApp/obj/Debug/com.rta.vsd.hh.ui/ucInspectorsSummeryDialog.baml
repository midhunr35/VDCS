   M S B A M L   `   `   `  ����|  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization  5      �{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5   	   A =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties 5      [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5      8   2VSDApp.com.rta.vsd.hh.ui.ucInspectorsSummeryDialog   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Z	 VWPFToolkit.Extended, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4UwpfxJhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit/extended 	 5      {DVrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit 	 5      �DVC{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit 	 5   	   0
propertiesclr-namespace:VSDApp.Properties 	 5      Iwf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms 	 5      -   5          >�Name$
  root��5	      $
��Auto��6   $
��Auto��6    � 	Resources 5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      �  � DataContext 6   ��  �� ElementName$
 root��.+�5   
   �� ���	#90000000$��0,0,0,0q�6J   y�6(   �� *
Visibility+._�5      �� -   5         ��Name$ 
stkPnlMain��5      $
��auto��6-   $
��auto��6:    �� Orientation$ Vertical=�6H   .+�5      �� -   5        $ stkPnlAllInspectors��5      $ 
Horizontal=�6:   .+�5      �� -   5        $ stkPnlMainInspector��5   !   $��5,25,8,0q�6>   
�������.+�5      �� -   5         ��Name$ imgInspector��5       $'��!../Images/Inspectors/ambadran.png��66   $	��100��6a   $
��auto��6m   �� 5      -   5        	 ��Name$	 lblInspectorRating��5   $   $��	 Rating: ��6@   
���   �$
��Bold"�6d   $��25(�6v   $
��Wrap=�6�   $��60��6�   $��1q�6�   5      >   8System.Windows.Controls.DataVisualization.Charting.Chart  5      -   5        $��mcChart��5   A   

 �� Top$
 80��6     �� Left$ 10��60   $	��850��6P   $	��350��6\   $��0,25,0,2q�6i   
�������   Series 5      E   ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  -   5          Name$ inspectionColumn��5   /   ?   9System.Windows.Controls.DataVisualization.Charting.Series   Title$ 
Inspection��6J   H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   IndependentValueBinding 6]   ��  �� Path$	 Key�   DependentValueBinding 6�   �� $ Value�   DataPointStyle 5   "   �� 5    &    �� 
TargetTypeH   BSystem.Windows.Controls.DataVisualization.Charting.ColumnDataPoint"  6,   .�5!   *   ��  �� Property � �� �� Value 5"   .   �� 5#   2    �� 
TargetType"  6B   .R�5$   6   � .+�5%   :   �� 
���   ���5&   >   �� 5'   B    �� EndPoint$ 0.5,1&�6V    �� 
StartPoint$ 0.5,0&�6g   .��5(   F   �  � Color$ #9BBB5A��6S    � Offset$ 0Q�6c   � 5)   F   $ #9BBB5A��6S   $ 1Q�6c   5*   C   5,   ;   � 5-   :   $��	0 -20 0 0q�6?   $��Center=�6R   $	��Top=�6o   .+�5.   >   �� B   <System.Windows.Controls.DataVisualization.Charting.DataPoint   FormattedDependentValue#��z 6H   $��0q�6y   $��10(�6�   5/   ;   50   7   51   3   53   +   54   '      LegendItemStyle 56   "   �� 57   &   C   =System.Windows.Controls.DataVisualization.Charting.LegendItem"  6,   .�58   *   ��  � �� 59   .   �� 5:   2   "  6B   .R�5<   6   �� .+�5=   :   �� $ 
Horizontal=�6E   .+�5>   >   �� $��6��6H   $��6��6R   ���#9BBB5A6]   �� 5?   >   $�� Inspections��6H   $
��Bold"�6\   ���#9BBB5A$��11(�6�   5A   ;   5B   7   5D   3   5F   +   5G   '   5I        5J      -	   5        $ violationColumn��5J   /   $ 	violation��6H    6Z   �� $	 Key� 6�   �� $ Value� 5K   "   �� 5L   &   "  6,   .�5M   *   ��  � �� 5N   .   �� 5O   2   "  6B   .R�5P   6   � .+�5Q   :   �� 
���   ���5R   >   �� 5S   B   $ 0.5,1&�6V   $ 0.5,0&�6g   .��5T   F   � $ #9B562A��6S   $ 0Q�6c   � 5U   F   $ #9B5B2D��6S   $ 1Q�6c   5V   C   5X   ;   � 5Y   :   $��	0 -20 0 0q�6?   $��Center=�6R   $	��Top=�6o   .+�5Z   >   �� #��z 6H   $��0q�6y   $��10(�6�   5[   ;   5\   7   5]   3   5_   +   5`   '    5b   "   �� 5c   &   "  6,   .�5d   *   ��  � �� 5e   .   �� 5f   2   "  6B   .R�5g   6   �� .+�5h   :   �� $ 
Horizontal=�6E   .+�5i   >   �� $��6��6H   $��6��6R   ���#9B5B2D6]   �� 5j   >   $�� Violations��6H   $
��Bold"�6[   ���#9B5B2D$��11(�6�   5l   ;   5m   7   5o   3   5q   +   5r   '   5t      5v      5w      �� 5x      -
   5        $ 
stkPnlBtns��5x      $ 
Horizontal=�61   $��Right=�6J   $��5,5,5,0q�6f   .+�5y      �� -   5        $ btnOk��5y      $y�	Collapsed=�6+   $&�� ..\Images\Buttons\Small\Back.png��6B   $	��100��6l   6�   �� 5z      -   5         ��Name$ btnBackImage2��5z      $��3��64   $��1��6A   !  VSDApp.Properties.Resources   BtnBack#��Z 6R   $��10,0,0,0q�6�      ButtonBackHalfControl#��[  6�   $��Right=�6�   6�   5{      5|      5}      5      5�      