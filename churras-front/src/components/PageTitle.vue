<template>
  <h1>{{ name }}</h1>
</template>

<script>
export default {
  name: "PageTitle",
  props: {
    name: String,
  },
};
</script>

<style scoped>
h1 {
  font-size: 2.4rem;
  font-weight: bold;
  text-align: center;
}
</style>