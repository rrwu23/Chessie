def twosum(nums: list[int], target: int):
   for i in range(len(nums)): #bad code
      for j in range(len(nums[i+1:])):
         if nums[i]+nums[j+i+1] == target:
            return [i, j+i+1]
   return 0


print(twosum([i for i in range(1000000)], 1999999))

# 19
# ();,
