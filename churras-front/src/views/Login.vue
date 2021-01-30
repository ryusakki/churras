<template>
  <div id="login">
    <page-title name="Agenda de churrasco" />
    <b-modal id="modal-registro" title="Cadastro" hide-footer>
      <b-form @submit="onRegister">
        <b-form-group
          label="Nome"
          label-for="cadastro-nome"
          class="font-weight-bold"
        >
          <b-form-input
            id="cadastro-nome"
            v-model="form['nome']"
            type="text"
            placeholder="Nome"
            required
          />
        </b-form-group>
        <b-form-group
          label="Email"
          label-for="cadastro-email"
          class="font-weight-bold"
        >
          <b-form-input
            id="cadastro-email"
            v-model="form['email']"
            type="email"
            placeholder="Email"
            required
          />
        </b-form-group>
        <b-form-group
          label="Senha"
          label-for="cadastro-senha"
          class="font-weight-bold"
        >
          <b-form-input
            id="cadastro-senha"
            v-model="form['senha']"
            type="password"
            placeholder="Senha"
            required
          />
        </b-form-group>
        <b-overlay
          opacity="0.6"
          spinner-small
          spinner-variant="primary"
          :show="registering"
          rounded="sm"
        >
          <b-button
            :disabled="registering"
            squared
            type="submit"
            block
            variant="success"
            >Efetuar cadastro</b-button
          >
        </b-overlay>
      </b-form>
    </b-modal>
    <b-form class="w-25" @submit="onSubmit">
      <b-form-group
        label="Login"
        label-for="input-login"
        class="font-weight-bold"
      >
        <b-form-input
          :disabled="loading"
          id="input-login"
          v-model="form['email']"
          type="email"
          placeholder="Email"
          required
        ></b-form-input>
      </b-form-group>
      <b-form-group
        label="Senha"
        label-for="input-senha"
        class="font-weight-bold"
      >
        <b-form-input
          :disabled="loading"
          id="input-senha"
          v-model="form['senha']"
          type="password"
          placeholder="Senha"
          required
        ></b-form-input>
      </b-form-group>
      <b-button :disabled="loading" type="submit" block variant="dark">
        <b-spinner v-if="loading" small></b-spinner> {{ btnLoginText }}
      </b-button>
      <hr class="mt-2 mb-3" />
      <b-link
        href
        v-b-modal.modal-registro
        :disabled="loading"
        class="text-dark d-block text-center"
      >
        Não possui uma conta? Cadastre-se!
      </b-link>
    </b-form>
  </div>
</template>

<script>
import PageTitle from "../components/PageTitle";
import usuarioService from "../services/UsuarioService";

export default {
  name: "Login",
  data() {
    return {
      registering: false,
      loading: false,
      btnLoginText: "Entrar",
      form: {},
    };
  },

  components: {
    "page-title": PageTitle,
  },

  methods: {
    onSubmit(event) {
      event.preventDefault();
      this.loading = !this.loading;
      this.btnLoginText = String.empty;

      const credenciais = JSON.parse(JSON.stringify(this.form));
      usuarioService
        .autenticar(credenciais)
        .then(() => this.$router.push("agenda"))
        .catch((e) => {
          this.loading = !this.loading;
          this.btnLoginText = "Entrar";

          const failure = {
            message: String.empty,
            title: "Falha",
            variant: String.empty,
            solid: true,
          };

          switch (e?.response?.status) {
            case 404:
            case 401:
              failure.message = "Credenciais inválidas";
              failure.variant = "warning";
              break;

            default:
              failure.message = "Serviço temporariamente indisponível";
              failure.variant = "danger";
              break;
          }
          this.$bvToast.toast(failure.message, failure);
        });
    },

    onRegister(event) {
      event.preventDefault();
      this.registering = !this.registering;

      const usuario = JSON.parse(JSON.stringify(this.form));
      usuarioService
        .registrar(usuario)
        .then(() => {
          this.registering = !this.registering;
          this.$bvModal.hide("modal-registro");
          this.$bvToast.toast("Conta criada", {
            title: "Ok",
            variant: "success",
            solid: true
          });
        })
        .catch((r) => {
          const toast = {
            message: String.empty,
            title: "Falha",
            variant: String.empty,
            solid: true,
          };
          switch (r?.response?.status) {
            case 409:
              toast.message = "Email em uso";
              toast.variant = "warning";
              break;
            default:
              toast.message = "Serviço temporariamente indisponível";
              toast.variant = "danger";
              break;
          }
          this.registering = !this.registering;
          this.$bvModal.hide("modal-registro");
          this.$bvToast.toast(toast.message, toast);
        });
    },
  },
};
</script>

<style scoped>
#login {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-evenly;
  width: 100%;
  height: 35em;
}
</style>