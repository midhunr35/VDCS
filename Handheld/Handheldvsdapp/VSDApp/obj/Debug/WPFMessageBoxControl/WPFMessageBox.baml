   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5   	   /    )VSDApp.WPFMessageBoxControl.WPFMessageBox   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6
propertiesclr-namespace:VSDApp.Properties     5   	   -   5        $	��250��5   
   $	��500��6   $	��175��6(     � 
ResizeMode$  NoResize=�5       � SizeToContent$ WidthAndHeight=�63    � WindowStyle$
 None=�6R    � ShowInTaskbar .  $��15(�5       � WindowStartupLocation$ CenterOwner=�5       � Title 5      �� *Title+ � 	Resources 5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      �� $��0q�6   
���  ��._�5      � 	 � RowDefinitions	 5   
   �� 5      $��55 �6   �� 5      $��2 �6   �� 5      $��* �6   �� 5      $��40 �6   
 � ColumnDefinitions
 5   
   �� 5      $��60 �6   �� 5      $��* �6   .+�5    
   �� $
��Left=�66   $��Center=�6Q   $��48��5!      $��48��6   $��10 0 0 0q�6'   ��5       �� *MessageImageSource+5!   '   �� 5"   
   $��2��6-   $��1��6>   $
��Wrap=�6N    �� TextAlignment$
 Left=�6b   $
��Left=�5#      $	��Top=�6(   $��10 10 10 10q�6@   ��5"      �� *	Message+5#   @   9� 5$   
   $��Show Details��6   $��2��6)   $��2��66   y�6J   �� *ShowDetails+.��5%      �� $��	15 4 10 4q�6   $
��Wrap=�6P   $
��Left=�5&      $	��Top=�63   $
 Left=�6K   ��5%   +   �� *InnerMessageDetails+5&   K   5'      �� 5(   
   $��3��6   $��1��6"    �� Orientation$ 
Horizontal=�62   $��Center=�6K   $��Right=�5)      $��0 0 5 0q�61   .+�5*      �� !   VSDApp.Properties.Resources
   Yes#��Z 6   
���  ��
�������   WPFMessageBoxButton#��[  6�   y�6C   �� *YesNoVisibility+��6�   �� *
YesCommand+6�   �� 5+      	   No#��Z 6   
�������
�������#��[  6�   y�6F   �� *YesNoVisibility+��6�   �� *	NoCommand+6�   �� 5,      #��Z 6   
���  ��
�������#��[  6�   y�6G   �� *CancelVisibility+��6�   �� *CancelCommand+6�   �� 5-      	   OK#��Z 6   
���  ��
�������#��[  6�   y�6F   �� *OkVisibility+��6�   �� *	OkCommand+6�   �� 5.      $��_Close��6   
�������
�������#��[  6�   y�6*   �� *CloseVisibility+��6Q   �� *CloseCommand+6�   5/      50      51      52      