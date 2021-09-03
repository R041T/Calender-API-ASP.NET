<template>
  <v-app>
    <v-app-bar app color="gray" dark>
      <div class="d-flex align-center">
        <v-img
          alt="Vuetify Logo"
          class="shrink mr-2"
          contain
          src="https://blog.trademarkia.com/wp-content/uploads/2020/10/Trademarkia_Logo-1-2.png"
          transition="scale-transition"
          width="300"
        />

        
      </div>

      <v-spacer></v-spacer>

    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script>
export default {
  name: "App",

  data: () => ({
    //
  }),
};
</script>
