import os, sys
import pandas as pd
import numpy as np


#pathの設定
if getattr(sys, 'frozen', False):    #Pyinstallerでビルドしたかどうか
    onore_no_basho = os.path.dirname(os.path.abspath(sys.executable))  #sys.executableがビルドしたEXEのパスを変換して返す
else:
    onore_no_basho = os.path.dirname(os.path.abspath(__file__)) #通常の自分のいる絶対パスを返す

path=onore_no_basho

#ファイル読み込み
headers=['Mo_ID','Ta_ID','Distance']
df=pd.read_csv(f'{path}/csv/toPython.csv',encoding='UTF-8',names=headers)

#mover_ID取得
col_list=df['Mo_ID'].unique()

#条件分岐
for i in range(len(col_list)):
    choi=df[df['Mo_ID']==col_list[i]]
    choi=choi.set_index('Ta_ID')
    choi=choi.rename(columns={'Distance':col_list[i]})
    choi=choi.drop('Mo_ID',axis=1)
    
    if  i ==0:
        prep_df=choi
    elif 1 <= i <= len(col_list):
        prep_df=pd.merge(prep_df,choi,how='outer',left_index=True,right_index=True)

#print(prep_df)
########################################################################################################
#Data_select_sprict
########################################################################################################

df_c=prep_df #コピー下で作業

#最小値をみつけて、そのMover_IDとTarget_IDを削除する関数
def func(df_c):
    new_df=pd.DataFrame()
    for col_name in df_c.columns:  #各moverの最短を探索するループ    
        total_min = df_c.min().min()  #df_c中の最小値を見つける
        df_pick=df_c[col_name].sort_values()   #moverの若いほうからソートする
        mover_min=df_pick.values[0]
        if total_min  == mover_min:  #分岐条件１：表中の最小値と指定のmoverでソートした時の最小値が同じだった場合
            mover_second=df_pick.values[1]
            if mover_min != mover_second:  #分岐条件２：そのmoverで2番目に近い経路が同じ重さでなかった場合
                new_df=new_df.append(df_pick.head(1)) #new_dfにmover、pick場所、重みを格納
                df_c=df_c.drop(df_pick.name,axis=1) #抽出したmoverをリストから除外
                df_c=df_c.drop(df_pick.index[0],axis=0) #抽出したTargetをリストから除外
            elif col_name == list(df_c.columns)[-1]:
                new_df=new_df.append(df_pick.head(1))   #最後に検索したmoverをアサイン
                df_c=df_c.drop(df_pick.name,axis=1)
                df_c=df_c.drop(df_pick.index[0],axis=0)
 
    return df_c, new_df


#関数を呼び出すループ
df_c, new_df=func(df_c)

for i in range(len(col_list)):
    if i == 0:
        final_new_df=new_df
    elif i == 1:
        if len(df_c.columns) != 0:
            df_c, new_dfx = func(df_c)
            final_new_df=pd.concat([new_df,new_dfx],axis=1)
    elif 2 <= i <= len(col_list):
        if len(df_c.columns) != 0:
            df_c, new_dfx = func(df_c)
            final_new_df=pd.concat([final_new_df,new_dfx],axis=1)
            

#index,columns名をリスト化
list_mo=list(final_new_df.index)
list_ta=list(final_new_df.columns)

#df形式に変更
out_data=np.array([list_mo,list_ta])
out_df=pd.DataFrame(out_data.T)
#print(out_df)
out_df=out_df.sort_values(0)
#print(out_df)

#保存と任意No.の出力
out_df.to_csv(f'{path}/csv/toCS.csv',index=False,header=False,encoding='UTF-8')
print(999)