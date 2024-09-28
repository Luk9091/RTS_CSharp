NUMBER_NAME=20;
FILE="Game/NameList.txt"

while [[ $NUMBER_NAME!=0 ]]; do
    rig | awk "NR==1{print $1}" >> $FILE;
    NUMBER_NAME=$(qalc "$NUMBER_NAME - 1")
done