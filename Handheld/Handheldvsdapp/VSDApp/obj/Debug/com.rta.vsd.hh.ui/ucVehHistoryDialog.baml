   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5      1    +VSDApp.com.rta.vsd.hh.ui.ucVehHistoryDialog   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Z VWPFToolkit.Extended, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4UwpfxJhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit/extended  5      0
propertiesclr-namespace:VSDApp.Properties  5      Iwf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms  5      -   5          >�Name$
  root��5      $
��Auto��6   $
��Auto��6    � 	Resources 5	       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      �� $
��Auto=�6   $
��Auto=�66   .��5   
   �  � DataContext 6   ��  �� ElementName$
 root��.+�5   
   �� ���	#90000000$��
0,0,0,-184q�6J   y�6(   �� *
Visibility+._�5      �� -   5         ��Name$ 	stkPnlGrd��5      $
��auto��6,   $
��auto��69    �� Orientation$ Vertical=�6G   .+�5      �� $
��Auto=�6   $
��Auto=�6O   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ grdViolationDetails��5      	   AutoGenerateColumns	 .  
   CanUserAddRows
 .     
IsReadOnly .  $
��auto=�5      $
��Auto=�6L   $   HorizontalScrollBarVisibility$ Visible=�6|   ��. $��15(�5      $��Segoe UI)�6'   $��Medium"�6=   $	��500��6Q   $	��400��6]   ��5      �� *violationData+ �� UpdateSourceTrigger$ PropertyChanged=�   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      !   VSDApp.Properties.Resources	   ID,  &System.Windows.Controls.DataGridColumn   Header# Z 61      
IsReadOnly .     Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6p      FontSize$ 15Q�6}      
FontFamily$ Segoe UI)�6�      
FontWeight$ Medium"�6�   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6�   �� *ViolationID+6�     5         	IssueDate# Z 61    .  $
 Auto 6y   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *	IssueDate+6�     5          Status# Z 61    .  $
 Auto 6v   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *ViolationStatus+6�     5!         Severity# Z 61    .  $
 Auto 6x   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *ViolationSeverity+6�     5"         PlateDetails# Z 61    .  $
 Auto 6{   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *VehicleDetail+6�     5#         ChassisNumber2# Z 61    .  $
 Auto 6}   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *ChassisNumber+6�     5$         MenueOperatorProfile# Z 62    .  $
 Auto 6�   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *OperatorDetail+6�   5.      5/      �� 50      $ 
Horizontal=�6   $��Right=�66   $��5,5,5,0q�6R   .+�52      �� -   5         ��Name$ btnOk��52         BtnBack#��Z 6-      ButtonBackHalfControl#��[  6_   $��35��6�   $��0,0,0,0q�6�   $��Right=�6�   6�   53      54      55      57      58      59      